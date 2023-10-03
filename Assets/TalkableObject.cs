using UnityEngine;
using UnityEngine.Events;

public class TalkableObject : MonoBehaviour
{
    [SerializeField] private NPCConfig_ScriptableObject config;

    public UnityEvent<NPCConfig_ScriptableObject> TalkEvent;

    public GameObject cam;  //Dialogue camera

    public void talk()
    {
        cam.SetActive(true);
        TalkEvent?.Invoke(config);
    }
    
}
