using UnityEngine;
using UnityEngine.Events;

public class TalkableObject : MonoBehaviour
{
    [SerializeField] private NPCConfig_ScriptableObject config;


    public UnityEvent<NPCConfig_ScriptableObject, TalkableObject> OnTalk;   // Event for when a player talks with the npc

    public GameObject cam;  //Dialogue camera
    
    public void DisableCamera()
    {
        this.cam.SetActive(false);
    }

    public void EnableCamera()
    {
        this.cam.SetActive(true);
    }


    public void talk(PlayerInteractionsController player)
    {
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                       this.transform.position.y,
                                       player.transform.position.z);
        this.transform.LookAt(targetPostition);
        OnTalk?.Invoke(config, this);
    }
    
}
