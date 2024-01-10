using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// A PlayerSlot consists of two gameobjects, the first one being an gameobject with a button which works
// as a background colour and the second one is the sprite of the object it contains.
public class PlayerSlotManager : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [Header("Important sprite transitions")]
    [SerializeField] private Sprite default_slotImage; // Default Visual representation of the slot
    [SerializeField] private Sprite focused_slotImage; // Visual representation of the slot when is focused
    [SerializeField] private Sprite selected_slotImage; // Visual representation of the slot when is Selected
    [SerializeField] private AudioClip focused_Audio; // Audio for slot focused
    [SerializeField] private AudioClip selected_Audio; // Audio for slot selected
    [SerializeField] private TextMeshProUGUI displayItemName; // Item name displayer

    [SerializeField] private AudioSource audioSource;   

    public static float SELECTED_BUTTON_RESIZE = 1;
    public static float FOCUSED_BUTTON_RESIZE = 0.3f;

    [SerializeField] private Image itemImage; // Visual representation of the item
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
        this.displayItemName.gameObject.SetActive(false);
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
        {
            this.currentStatus = SlotState.UNSELECTED;
        }
            
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (this.currentStatus != SlotState.SELECTED)
        {
            audioSource.PlayOneShot(this.focused_Audio);
            this.currentStatus = SlotState.FOCUSED;
        }
            
    }

    public void OnSubmit(BaseEventData eventData)
    {
        audioSource.PlayOneShot(this.selected_Audio);
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
        this.itemImage.gameObject.SetActive(true);
        this.itemSO = item;

        if (item == null)
        {
            this.itemImage.gameObject.SetActive(false);
            displayItemName.text = "";
        }
        else {
            this.itemImage.sprite = item.ItemSprite;
            displayItemName.text = item.Name;
        }
    }

    public void removeItemSO()
    {
        this.slotManager.removeItem(this.itemSO);
        this.itemSO = null;
        this.itemImage.gameObject.SetActive(false);
    }

    public void EnableObjectName()
    {
        this.displayItemName.gameObject.SetActive(true);
    }

    public void DisableObjectName()
    {
        this.displayItemName.gameObject.SetActive(false);
    }
}
