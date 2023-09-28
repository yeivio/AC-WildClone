using UnityEngine;
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

    private void OnEnable()
    {
        initialSelectedbtn.Select();
    }


    public bool AddItem(Sprite sprite)
    {
        foreach (PlayerSlotManager slot in inventorySlots)
        {
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
