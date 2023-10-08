using UnityEngine;
using UnityEngine.Events;

public class TalkableObject : MonoBehaviour
{
    [SerializeField] private NPCConfig_ScriptableObject config;

    public UnityEvent<NPCConfig_ScriptableObject> OnTalk;

    public GameObject cam;  //Dialogue camera

    private void Start()
    {
        //FindObjectByType<Npc_Dialogue>().OnClose.AddListener(DisableCamera);
        FindAnyObjectByType<Npc_Dialogue>(FindObjectsInactive.Include).OnClose.AddListener(DisableCamera);
    }
    
    private void DisableCamera()
    {
        this.cam.SetActive(false);
    }

    public void talk()
    {
        cam.SetActive(true);
        OnTalk?.Invoke(config);
    }
    
}
