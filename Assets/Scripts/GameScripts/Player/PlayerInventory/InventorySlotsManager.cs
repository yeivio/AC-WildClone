using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsManager : MonoBehaviour
{
    [SerializeField] private PlayerSlotManager[] inventorySlots; // Inventory slots
    [SerializeField] private PlayerInventory_ScriptableObject inventoryData; //Player inventory container
    private List<InventoryItem_ScriptableObject> inventoryDataCopy;
    public PlayerSlotManager currentSelectedObject;

    private void OnDisable()
    {
        currentSelectedObject = null;
    }

    private void OnEnable()
    {
        inventoryDataCopy = new List<InventoryItem_ScriptableObject>(inventoryData.getList());

        // Loads the player inventory contained in the SO
        foreach (PlayerSlotManager slot in inventorySlots)
            if (slot.GetItemSO() != null)
            {
                if (this.inventoryDataCopy.Contains(slot.GetItemSO()))
                {
                    this.inventoryDataCopy.Remove(slot.GetItemSO());
                }
                else
                {
                    slot.resetItemSO();
                }
                
            }

        int index = 0;
        while(inventoryDataCopy.Count != 0)
        {
            if(index >= 31)
            {
                throw new System.Exception("Error al importar los datos");
            }
            if (this.inventorySlots[index].GetItemSO() == null) {
                this.inventorySlots[index].SetItemSO(this.inventoryDataCopy[0]);
                this.inventoryDataCopy.RemoveAt(0);
                
            }
            index++;
        }
        

            





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
