using NUnit.Framework.Internal;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerSlotManager[] inventorySlots = new PlayerSlotManager[30];
   
    [SerializeField] private DialogManager dialogWindow; // Dialog window reference
    [SerializeField] private GameObject inventoryBackground; // Inventory Frame object

    private PlayerInput playerInput; //Player input reference

    [SerializeField] private PlayerInventory_ScriptableObject inventoryData; //Player inventory container


    private void Start()
    {
        inventorySlots[0].GetComponent<Button>().Select(); // Select first button

        playerInput = FindFirstObjectByType<PlayerInput>();
        inventoryData.OnAddItem.AddListener(AddItem);
        this.dialogWindow.OnClose.AddListener(DialogClose);
        this.dialogWindow.OnItemDrop.AddListener(this.RemoveItem);
        this.dialogWindow.gameObject.SetActive(false);
        this.inventoryBackground.SetActive(false);
    }

    private void AddItem(InventoryItem_ScriptableObject item, int index){ this.inventorySlots[index].SwitchItem(item); }
    private void RemoveItem(InventoryItem_ScriptableObject item) { inventoryData.DeleteItem(item); }

    private void DialogClose(PlayerSlotManager slot) { slot.gameObject.GetComponent<Button>().Select(); }
    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInput.SwitchCurrentActionMap(Utils.UI_INPUTMAP);
            this.inventoryBackground.SetActive(true);
        }
    }

    public void OpenDialogWindow(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        PlayerSlotManager obj = EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>();
        if (obj.itemSO) { 
            this.dialogWindow.gameObject.SetActive(true);
        }
    }


    public void CloseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (dialogWindow.gameObject.activeSelf) { // Dialog confirmation window active
            dialogWindow.gameObject.SetActive(false);
            inventorySlots[0].GetComponent<Button>().Select();
        }
        else {
            this.dialogWindow.gameObject.SetActive(false);
            this.inventoryBackground.SetActive(false);
            playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
        }
    }
}
