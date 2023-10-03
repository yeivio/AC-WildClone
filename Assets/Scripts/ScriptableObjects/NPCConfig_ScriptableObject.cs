using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName ="Npc_ScriptableObject", menuName = "Custom Assets/NPCConfig")]
public class NPCConfig_ScriptableObject : ScriptableObject
{
    public UnityEvent<string, string> OnNPCInteract;

    public string npc_name;
    public string dialogue;
    [SerializeField] private string dialogue_text { get; set; }

    public void NPCInteract()
    {
        OnNPCInteract.Invoke(this.npc_name, this.dialogue);
    }

}
