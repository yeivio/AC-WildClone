using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingUI_Manager : MonoBehaviour
{
    [Header("Mandatory Objects")]
    [SerializeField] private RectTransform startingPoint; // First pivoting point
    [SerializeField] private RectTransform endingPoint; // Second pivoting point
    [SerializeField] private RectTransform cursor; // Object with the cursor sprite
    [SerializeField] private RectTransform jackpot; // Object with the winnable zone 
    private float lerpSpeed = 1; // Speed of the cursor
    private bool keyPressed;
    private PlayerInputController playerInput;
    [SerializeField] private PlayerInventory_ScriptableObject playerInventory;
    [SerializeField] private InventoryItem_ScriptableObject fish;
    [SerializeField] private AudioSource captureAudio;
    [SerializeField] private AudioSource captureFailedAudio;


    private bool isCoroutineRunning;    // In case the object is called multiple times at the same time.
    private bool isFinishAnimationRunning;


    private void Start()
    {
        this.isCoroutineRunning = false;
        isFinishAnimationRunning = false;
        playerInput = FindAnyObjectByType<PlayerInputController>();
    }

    private void OnEnable()
    {
        if (!isCoroutineRunning) { 
            keyPressed = false;
            StartCoroutine(FishingAnimation());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines(); // Force stop of coroutine
        playerInput.SwitchInputMap(Utils.FREEMOVE_INPUTMAP);
        isCoroutineRunning = false;
    }
    /// <summary>
    /// Función para cuando el jugador quiere parar el objeto en movimiento. Si el objeto se encuentra dentro del area del
    /// objeto jackpot. Si es así lo que ocurre es que se añade el objeto al inventorio del jugador, se desactiva el objeto
    /// y se devuelve el control al jugador. 
    /// Si no es así hará lo mismo pero sin darle el objeto al jugador.
    /// </summary>
    /// <param name="context"></param>
    public void action(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        keyPressed = true;
        if (CheckWin() && !isFinishAnimationRunning) {
            isFinishAnimationRunning = true;
            StartCoroutine(AnimationFinishWin());
        }
        else
        {
            if (!isFinishAnimationRunning)
            {
                isFinishAnimationRunning = true;
                StartCoroutine(AnimationFinishLose());
            }
            
        }
    }


    /// <summary>
    /// Comprueba si el objeto que usamos como cursor que se mueve está dentro del objeto que usamos como 
    /// zona de victoria. 
    /// </summary>
    private bool CheckWin()
    {
        return cursor.position.x <= (jackpot.position.x + jackpot.rect.width / 2) &&
            cursor.position.x >= (jackpot.position.x - jackpot.rect.width / 2);
    }
    IEnumerator FishingAnimation()
    {
        isCoroutineRunning = true;
        while (!keyPressed)
        {
            float time = 0;

            while (time < 1 && !keyPressed) // Going right animation
            {
                cursor.transform.position = Vector3.Lerp(startingPoint.position, endingPoint.position, time);
                time += Time.deltaTime * lerpSpeed;
                yield return null;
            }

            time = 0;

            while (time < 1 && !keyPressed) // Going left animation
            {
                cursor.transform.position = Vector3.Lerp(endingPoint.position, startingPoint.position, time);
                time += Time.deltaTime * lerpSpeed;
                yield return null;
            }
        }
        isCoroutineRunning = false;
    }

    IEnumerator AnimationFinishWin()
    {
        
        playerInventory.AddItem(fish);
        captureAudio.Play();
        yield return new WaitForSeconds(2f);
        playerInput.SwitchInputMap(Utils.FREEMOVE_INPUTMAP);
        isFinishAnimationRunning = false;
        this.gameObject.SetActive(false);
        
    }
    IEnumerator AnimationFinishLose()
    {
        
        captureFailedAudio.Play();
        yield return new WaitForSeconds(1f);
        playerInput.SwitchInputMap(Utils.FREEMOVE_INPUTMAP);
        isFinishAnimationRunning = false;
        this.gameObject.SetActive(false);
        
    }


}
