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

    public Sprite appleSprite;
    [SerializeField] private Button initialSelectedbtn;
    [SerializeField] private Button[] allMenuBtn;

    private PlayerSlotManager selectedItem; // Item selected by player


    private void OnEnable()
    {
        initialSelectedbtn.Select();
    }

    private void Start()
    {
        allMenuBtn = this.GetComponentsInChildren<Button>();
        foreach (Button btn in allMenuBtn) {
            btn.onClick.AddListener(itemSelected);
        }
    }

    private void itemSelected()
    {
        PlayerSlotManager selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>();

        if(selectedItem && selectedButton.hasItem()) //Intercambio de sprites
        {
            Sprite oldSprite = selectedButton.removeItem(); 
            selectedButton.addItem(selectedItem.getItem());
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
        else if (selectedButton.hasItem())
            selectedItem = selectedButton;

    }

    public bool AddItem(Sprite sprite)
    {
        foreach (PlayerSlotManager slot in inventorySlots)
        {
            Debug.Log(slot);
            if (!slot.hasItem())
            {
                slot.addItem(sprite);
                return true;
            }
        }

        return false;
    }


    public void CloseMenu(InputAction.CallbackContext context)
    {
        this.gameObject.SetActive(false);
        playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
    }
}
