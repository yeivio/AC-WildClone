using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// A PlayerSlot consists of two gameobjects, the first one being an gameobject with a button which works
// as a background colour and the second one is the sprite of the object it contains.
public class PlayerSlotManager : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [Header("Important sprite transitions")]
    [SerializeField] private Sprite default_slotImage; // Default Visual representation of the slot
    [SerializeField] private Sprite focused_slotImage; // Visual representation of the slot when is focused
    [SerializeField] private Sprite selected_slotImage; // Visual representation of the slot when is Selected
  
    public static float SELECTED_BUTTON_RESIZE = 1;
    public static float FOCUSED_BUTTON_RESIZE = 0.3f;

    private Image itemImage; // Visual representation of the item
    private InventoryItem_ScriptableObject itemSO; // Item contained on the slot
    private InventorySlotsManager slotManager;

    private enum SlotState { FOCUSED, SELECTED, UNSELECTED};
    private SlotState currentStatus;
    private Vector3 originalSize; // Original button size


    private void Start()
    {
        currentStatus = SlotState.UNSELECTED;
        slotManager = GetComponentInParent<InventorySlotsManager>();
        itemImage =  this.transform.GetChild(0).GetComponentInChildren<Image>();
        originalSize = this.GetComponent<Button>().transform.localScale;  // Save the original size
    }

    private void OnDisable()
    {
        this.SetItemSO(null); // resets items
        this.currentStatus = SlotState.UNSELECTED;
    }

    private void Update()
    {
        switch(currentStatus){
            case SlotState.SELECTED:
                this.GetComponent<Image>().sprite = selected_slotImage;
                this.GetComponent<Button>().GetComponent<RectTransform>().localScale =
                    new Vector3(originalSize.x + SELECTED_BUTTON_RESIZE, originalSize.y + SELECTED_BUTTON_RESIZE, originalSize.z); // Rescale button
                break;

            case SlotState.UNSELECTED:
                this.GetComponent<Button>().GetComponent<RectTransform>().localScale = originalSize; //Revert size transform
                this.GetComponent<Image>().sprite = default_slotImage;
                break;
            case SlotState.FOCUSED:
                this.GetComponent<Image>().sprite = focused_slotImage;
                this.GetComponent<Button>().GetComponent<RectTransform>().localScale =
                    new Vector3(originalSize.x + FOCUSED_BUTTON_RESIZE, originalSize.y + FOCUSED_BUTTON_RESIZE, originalSize.z); // Rescale button
                break;

            default:
                break;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(this.currentStatus != SlotState.SELECTED)
            this.currentStatus = SlotState.UNSELECTED;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (this.currentStatus != SlotState.SELECTED)
            this.currentStatus = SlotState.FOCUSED;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        this.currentStatus = SlotState.SELECTED;
        this.GetComponent<Image>().sprite = selected_slotImage;
        slotManager.ItemSelect(this);
    }

    public void Deselect()
    {
        this.currentStatus = SlotState.UNSELECTED;
        OnDeselect(null);
    }


    public InventoryItem_ScriptableObject GetItemSO()
    {
        return this.itemSO;
    }

    public void SetItemSO(InventoryItem_ScriptableObject item)
    {
        if(itemImage == null) //ItemImage not registered
            itemImage = this.transform.GetChild(0).GetComponentInChildren<Image>();
        this.itemImage.gameObject.SetActive(true);
        this.itemSO = item;

        if (item == null)
            this.itemImage.gameObject.SetActive(false);
        else
            this.itemImage.sprite = item.ItemSprite;   
    }

    public void removeItemSO()
    {
        this.itemSO = null;
        this.itemImage.gameObject.SetActive(false);
    }
}
