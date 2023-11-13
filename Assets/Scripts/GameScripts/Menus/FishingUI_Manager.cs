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
    private PlayerInput playerInput;
    [SerializeField] private PlayerInventory_ScriptableObject playerInventory;
    [SerializeField] private InventoryItem_ScriptableObject fish;

    private bool isCoroutineRunning;    // In case the object is called multiple times at the same time.


    private void Start()
    {
        this.isCoroutineRunning = false;
        playerInput = FindAnyObjectByType<PlayerInput>();
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
        if (CheckWin()) { 
            playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
            this.gameObject.SetActive(false);
            playerInventory.AddItem(fish);
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
}
