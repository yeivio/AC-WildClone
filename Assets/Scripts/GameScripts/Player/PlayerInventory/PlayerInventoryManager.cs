using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerSlotManager[] inventorySlots = new PlayerSlotManager[30];
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject dialogWindow;

    public Sprite appleSprite;

    private PlayerSlotManager selectedItem; // Item selected by player

    [SerializeField] private PlayerInventory_ScriptableObject inventoryData; //Player inventory 


    private void OnEnable()
    {
        inventorySlots[0].GetComponent<Button>().Select();

        int contador = 0;
        foreach (InventoryItem_ScriptableObject aux in inventoryData.inventoryItems) {
            inventorySlots[contador].SwitchItem(aux.ItemSprite);
            contador++;
        }
    }

    public void CloseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (dialogWindow.activeSelf) { // Dialog confirmation window
            dialogWindow.SetActive(false);
            inventorySlots[0].GetComponent<Button>().Select();
        }
        else {
            this.gameObject.SetActive(false);
            playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
        }
    }
    public void OpenDialog(InputAction.CallbackContext context)
    {
        //if (!context.performed || EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>().hasItem())
        //    return;
        //initialSelectedbtn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>(); // Saving last selected slot
        //this.dialogWindow.SetActive(true);
    }
}
