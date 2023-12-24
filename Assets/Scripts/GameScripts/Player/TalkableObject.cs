using UnityEngine;

public class TalkableObject : MonoBehaviour
{
    [SerializeField] private NPCConfig_ScriptableObject config;
    [SerializeField] private Npc_Dialogue npcText_UI;

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
        //Npc looks at player
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                       this.transform.position.y,
                                       player.transform.position.z);
        this.transform.LookAt(targetPostition);

        npcText_UI.gameObject.SetActive(true);
        npcText_UI.StartDialogueBox(config, this);
    }
    
}
