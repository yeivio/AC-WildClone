using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerSlotManager[] inventorySlots = new PlayerSlotManager[30];
    [SerializeField] private MenuManager menuManager;


    public Sprite appleSprite;
    public Button btn;




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

}
