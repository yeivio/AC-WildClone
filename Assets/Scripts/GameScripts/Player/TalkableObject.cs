using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TalkableObject : MonoBehaviour
{
    [SerializeField] private NPCConfig_ScriptableObject [] config;
    [SerializeField] private Npc_Dialogue npcText_UI;
    [SerializeField] private AudioClip npcTalkAudio;


    public GameObject cam;  //Dialogue camera
    public int actualTalk;  // Next dialog to be carried
    private Animator playerAnimator;
    public bool tiendaAbierta; // Look for an open shop menu
    public bool isBuyShop; // if true the open shop is a buy shop, if false it is a sell shop
        
    public bool isActive;  // Control variable to know which TalkableObj is the player currently talking

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        actualTalk = 0;
        tiendaAbierta = false;
    }
    public void Continue()
    {
        Debug.Log("HOLA, POR SEGUNDA VEZ");
        if (tiendaAbierta == true || !isActive)
            return;
        Debug.Log($"CONTINUE INPUT ACTION {tiendaAbierta}");
        npcText_UI.ContinueDialog(config[actualTalk]);
        actualTalk += 1;
    }
    public void Continue(string toAdd)
    {
        string dialog;
        if (isBuyShop)
            dialog = "Pues el precio es: ";
        else
            dialog = "Te lo compro por: ";
        dialog += toAdd;
        npcText_UI.AfterTienda(dialog);
    }
    public void DisableCamera()
    {
        this.cam.SetActive(false);
        playerAnimator.SetBool("isTalking", false);
        playerAnimator.SetBool("isRotating", false);
    }

    public void EnableCamera()
    {
        this.cam.SetActive(true);
        StopAllCoroutines();
    }

    public void talk(PlayerInteractionsController player)
    {
        //Npc looks at player
        isActive = true;
        actualTalk = 0;
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                       this.transform.position.y,
                                       player.transform.position.z);
        StartCoroutine(rotateToPlayer(targetPostition));
    }

    IEnumerator rotateToPlayer(Vector3 position)
    {
        //https://stackoverflow.com/questions/61768693/face-other-object-using-lerp-in-a-coroutine

        float time = 0f;
        float duration = 0.5f;
        Quaternion startValue = transform.rotation;
        Vector3 direction = position - transform.position;
        Quaternion endValue = Quaternion.LookRotation(direction);
        playerAnimator.SetBool("isRotating", true);
        
        while(time <= 1f)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time);
            time += Time.deltaTime / duration;
            yield return null;
        }

        transform.rotation = endValue;
        playerAnimator.SetBool("isTalking", true);
        
        npcText_UI.gameObject.SetActive(true);
        Debug.Log(actualTalk);
        npcText_UI.StartDialogueBox(config[actualTalk], npcTalkAudio, this);
        actualTalk = 1; // next talk to be done
    }

}
