using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerSlotManager[] inventorySlots = new PlayerSlotManager[30];

    public Sprite appleSprite;

    public bool addItem(Sprite sprite)
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

    public bool addItem()
    {
        foreach (PlayerSlotManager slot in inventorySlots)
        {
            if (!slot.hasItem())
            {
                slot.addItem(appleSprite);
                Debug.Log("true");
                return true;
            }
        }
        Debug.Log("false");
        return false;
    }

}
