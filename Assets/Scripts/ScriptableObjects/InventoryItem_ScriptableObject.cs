using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItems_ScriptableObject", menuName = "Custom Assets/InventoryItems")]
public class InventoryItem_ScriptableObject : ScriptableObject
{
    [field: SerializeField] public bool IsStackable { get; set; }
    [field: SerializeField] public int MaxStackSize { get; set; } = 1;
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Sprite ItemSprite{ get; set; }
    public int ID => GetInstanceID();

}
