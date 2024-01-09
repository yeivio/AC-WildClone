using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsManager : MonoBehaviour
{
    [SerializeField] private PlayerSlotManager[] inventorySlots; // Inventory slots
    [SerializeField] private PlayerInventory_ScriptableObject inventoryData; //Player inventory container

    private PlayerSlotManager currentSelectedObject;

    private void OnEnable()
    {
        // Loads the player inventory contained in the SO
        for (int i = 0; i < inventoryData.getList().Count &&  i < inventorySlots.Length; i++)
            inventorySlots[i].SetItemSO(inventoryData.getList()[i]);
        this.FocusItem(0);// Autoselect the first button for navigation
    }

    /// <summary>
    /// Function which makes the inventory slot given through parameter the one focussing right now. 
    /// If the number is out of ranges, the default value will be the first one.
    /// </summary>
    /// <param name="invNumber"></param>
    public PlayerSlotManager FocusItem(int invNumber)
    {
        if (invNumber < 0 && invNumber >= inventorySlots.Length)
            invNumber = 0; //Out of range

        this.inventorySlots[invNumber].OnSelect(null);
        this.inventorySlots[invNumber].gameObject.GetComponent<Button>().Select();
        return this.inventorySlots[invNumber];
    }


    /// <summary>
    /// Function when an slot is selected. Every 2 clicks we swap the objects between the slots and visually reset the first one
    /// </summary>
    /// <param name="slot"></param>
    public void ItemSelect(PlayerSlotManager slot)
    {
        if(currentSelectedObject != null)
        { //Swap objects
            InventoryItem_ScriptableObject aux = currentSelectedObject.GetItemSO();
            currentSelectedObject.SetItemSO(slot.GetItemSO());
            slot.SetItemSO(aux);
            currentSelectedObject.Deselect(); // Reset last object clicked�
            // Reset current selected object
            slot.Deselect();
            slot.OnSelect(null); 
            currentSelectedObject = null;
        }
        else { 
            currentSelectedObject = slot;
        }
    }


    public void removeItem(InventoryItem_ScriptableObject item)
    {
        if (item != null)
            this.inventoryData.DeleteItem(item);
    }
}
