using System.Collections;
using UnityEngine;

public class TalkableObject : MonoBehaviour
{
    [SerializeField] private NPCConfig_ScriptableObject config;
    [SerializeField] private Npc_Dialogue npcText_UI;
    [SerializeField] private AudioClip npcTalkAudio;


    public GameObject cam;  //Dialogue camera
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
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
        npcText_UI.StartDialogueBox(config, npcTalkAudio, this);

    }

}
