using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerInventory_ScriptableObject", menuName = "Custom Assets/Inventory")]
public class PlayerInventory_ScriptableObject : ScriptableObject
{
    [field: SerializeField] public List<InventoryItem_ScriptableObject> inventoryItems; // Already on inventory
    [field: SerializeField] public int Size { get; private set; } = 30;

    public UnityEvent<InventoryItem_ScriptableObject,int> OnAddItem;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem_ScriptableObject>();
    }
    public bool AddItem(InventoryItem_ScriptableObject newItem)
    {
        if (inventoryItems.Count >= Size)
            return false;
        inventoryItems.Add(newItem);
        OnAddItem?.Invoke(newItem, inventoryItems.Count-1);
        return true;
    }

    public bool DeleteItem(InventoryItem_ScriptableObject delItem)
    {
        return inventoryItems.Remove(delItem);
    }

}
