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
        inventoryData.OnAddItem.AddListener(addItem);
        this.dialogWindow.OnClose.AddListener(dialogClose);
        this.dialogWindow.OnItemDrop.AddListener(this.RemoveItem);
        this.dialogWindow.gameObject.SetActive(false);
        this.inventoryBackground.SetActive(false);
    }

    private void addItem(InventoryItem_ScriptableObject item, int index)
    {
        this.inventorySlots[index].SwitchItem(item);
    }

    private void RemoveItem(InventoryItem_ScriptableObject item) { inventoryData.DeleteItem(item); }

    private void dialogClose() { inventorySlots[0].GetComponent<Button>().Select(); }
    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInput.SwitchCurrentActionMap(Utils.UI_INPUTMAP);
            this.inventoryBackground.SetActive(true);

            /*
            int contador = 0;
            foreach (InventoryItem_ScriptableObject aux in inventoryData.inventoryItems)
            { //Update player inventory
                inventorySlots[contador].SwitchItem(aux.ItemSprite);
                contador++;
            }   */
        }
    }

    public void OpenDialogWindow(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        PlayerSlotManager obj = EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>();
        this.dialogWindow.gameObject.SetActive(true);
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
