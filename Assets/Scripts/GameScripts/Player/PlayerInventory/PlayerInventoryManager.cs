using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//This class is the manager in charge of opening and closing the player inventories menus
public class PlayerInventoryManager : MonoBehaviour
{

    [Header("Mandatory Assets")]
    [SerializeField] private InventorySlotsManager invSlotsManager; // Player inventory slots manager
    [SerializeField] private DialogManager dialogWindow; // Dialog window reference
    [SerializeField] private GameObject inventoryBackground; // GameObject which contains the visuals of the player inventory

    private PlayerInput playerInput; //Player input reference

    private void Start()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();

        // Hide all the UI
        this.dialogWindow.gameObject.SetActive(false);  
        this.inventoryBackground.SetActive(false);   
    }

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
        if (EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>().GetItemSO()) { 
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
            invSlotsManager.FocusItem(0);
        }
        else {
            this.dialogWindow.gameObject.SetActive(false);
            this.inventoryBackground.SetActive(false);
            playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
        }
    }
}
