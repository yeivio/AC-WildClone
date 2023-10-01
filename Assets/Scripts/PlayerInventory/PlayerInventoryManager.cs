using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerSlotManager[] inventorySlots = new PlayerSlotManager[30];
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject dialogWindow;

    public Sprite appleSprite;
    [SerializeField] private Button initialSelectedbtn;

    private PlayerSlotManager selectedItem; // Item selected by player

    private void OnEnable()
    {
        initialSelectedbtn.Select();
    }

    private void Start()
    {
        foreach (PlayerSlotManager btn in inventorySlots) {
            btn.gameObject.GetComponent<Button>().onClick.AddListener(itemSelected);
        }

    }

    private void itemSelected()
    {
        PlayerSlotManager selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>();

        if(selectedItem && selectedButton.hasItem()) //Intercambio de sprites
        {
            selectedItem.UnClick();
            Sprite oldSprite = selectedButton.removeItem(); 
            selectedButton.addItem(selectedItem.removeItem());
            selectedItem.addItem(oldSprite);
            selectedItem = null;
        }
        else if (selectedItem)
        {
            selectedButton.addItem(selectedItem.removeItem());
            selectedButton.resizeItem();
            selectedItem.revertSizeItem();
            selectedItem = null;
        }
        else if (selectedButton.hasItem()) { 
            selectedItem = selectedButton;
        }

    }

    public bool AddItem(Sprite sprite)
    {
        int contador = 1;
        foreach (PlayerSlotManager slot in inventorySlots)
        {
            if (!slot.hasItem())
            {
                slot.addItem(sprite);
                return true;
            }
            contador++;
        }
        return false;
    }

    public void CloseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (dialogWindow.activeSelf) { 
            dialogWindow.SetActive(false);
            initialSelectedbtn.Select();
        }
        else {
            this.gameObject.SetActive(false);
            playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
        }
    }
    public void OpenDialog(InputAction.CallbackContext context)
    {
        if (!context.performed || EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>().hasItem())
            return;
        initialSelectedbtn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>(); // Saving last selected slot
        this.dialogWindow.SetActive(true);
    }
}
