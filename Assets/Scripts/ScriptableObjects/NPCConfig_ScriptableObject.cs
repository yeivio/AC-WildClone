using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName ="Npc_ScriptableObject", menuName = "Custom Assets/NPCConfig")]
public class NPCConfig_ScriptableObject : ScriptableObject
{
    public string npc_name;
    public string dialogue;
    [SerializeField] private string dialogue_text { get; set; }

}
