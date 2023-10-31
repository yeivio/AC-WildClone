using NUnit.Framework.Internal;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//This class is the manager in order to keep the PlayerInventory_SO, and activate and deactivate the player inventory UI and the dialog windows
public class PlayerInventoryManager : MonoBehaviour
{

    [Header("Mandatory Assets")]
    [SerializeField] private PlayerSlotManager[] inventorySlots = new PlayerSlotManager[30];
    [SerializeField] private DialogManager dialogWindow; // Dialog window reference
    [SerializeField] private GameObject inventoryBackground; // GameObject which contains the visuals of the player inventory
    [SerializeField] private PlayerInventory_ScriptableObject inventoryData; //Player inventory container

    private PlayerInput playerInput; //Player input reference

    private void Start()
    {
        inventorySlots[0].GetComponent<Button>().Select(); // Autoselect the first button for navigation

        playerInput = FindFirstObjectByType<PlayerInput>();
        /* Event subscibe */
        this.inventoryData.OnAddItem.AddListener(this.AddItem); 
        this.dialogWindow.OnClose.AddListener(this.DialogClose);

        this.dialogWindow.gameObject.SetActive(false);  // Hide the DialogWindow UI
        this.inventoryBackground.SetActive(false);   // Hide the player inventory UI
    }

    /// <summary>
    /// When an item is added to the playerInventory SO we pick the PlayerSlot of the given index and load the item SO
    /// </summary>
    private void AddItem(InventoryItem_ScriptableObject item, int index){ this.inventorySlots[index].SwitchItem(item); }
    
    /// <summary>
    /// When an item is deleted from a playerslot, we update and remove the player inventory SO
    /// </summary>
    public void RemoveItem(InventoryItem_ScriptableObject item) { inventoryData.DeleteItem(item); }

    
    /// <summary>
    /// When the dialog is closed, we force select the slot we were. This is to keep the navigation.
    /// </summary>
    private void DialogClose(PlayerSlotManager slot) { slot.gameObject.GetComponent<Button>().Select(); }

    /// <summary>
    /// When the player press the open inventory button, we switch the Action map and display the player inventory UI
    /// </summary>
    public void OpenInventory(InputAction.CallbackContext context)
    {
        playerInput.SwitchCurrentActionMap(Utils.UI_INPUTMAP);
        this.inventoryBackground.SetActive(true);
    }

    /// <summary>
    /// Function for when the player clicks on a PlayerSlotManager. If the PlayerSlotManager contains an item, the function enable
    /// the dialogwindow UI
    /// </summary>
    public void OpenDialogWindow(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>().itemSO) { 
            this.dialogWindow.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// When the player press the button for Closing a menu we differentiate if a dialog window is active, which will only close 
    /// the dialog window and reset the selected PlayerSlot for navigation. And if there isn't any dialog window active, which will
    /// disable the UI and change the inputmap to FreeMove 
    /// </summary>
    public void CloseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (dialogWindow.gameObject.activeSelf) { // If dialogwindow is active, we only close dialogWindow
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
