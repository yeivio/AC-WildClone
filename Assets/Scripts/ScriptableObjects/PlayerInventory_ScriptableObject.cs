using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Custom dialog menu
[CreateAssetMenu(fileName = "PlayerInventory_ScriptableObject", menuName = "Custom Assets/Inventory")]
public class PlayerInventory_ScriptableObject : ScriptableObject
{
    private List<InventoryItem_ScriptableObject> inventoryItems; // List with the items the player currently have
    [field: SerializeField] public int Size { get; private set; } = 30; // Default max numbers of items the player can save

    public UnityEvent<InventoryItem_ScriptableObject, int> OnAddItem;
    public UnityEvent<InventoryItem_ScriptableObject, int> OnRemoveItem;

    public void Awake()
    {
        inventoryItems = new List<InventoryItem_ScriptableObject>();
    }
    /// <summary>
    /// Save a new item on the player inventory. If the item is added, an OnAddItem event will trigger with the 
    /// new item added and the inventory size as parameters.
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns>True if the object is saved, false if the object couldn't be saved because the inventory
    /// doesn't have enough space</returns>
    public bool AddItem(InventoryItem_ScriptableObject newItem)
    {

        if (inventoryItems.Count >= Size)
            return false;
        inventoryItems.Add(newItem);
        OnAddItem?.Invoke(newItem, inventoryItems.Count - 1);
        return true;
    }

    public bool DeleteItem(InventoryItem_ScriptableObject delItem)
    {
        if (inventoryItems.Remove(delItem))
        {
            OnRemoveItem?.Invoke(delItem, inventoryItems.Count - 1);
        }
        return false;
    }
    public List<InventoryItem_ScriptableObject> getList() { return this.inventoryItems; }
}