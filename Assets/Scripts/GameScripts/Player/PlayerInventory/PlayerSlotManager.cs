using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSlotManager : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    private Vector3 originalSize; // Original size

    [SerializeField] private Image itemObject; // Sprite of the item contained
    [SerializeField] private Button buttonObject; // The button Object
    [SerializeField] private Sprite selectedSprite; // Sprite when object is hovered by
    [SerializeField] private Sprite pressedSprite; // Sprite when an slot is clicked
    [SerializeField] private Sprite normalSprite; // Sprite when idle mode
    [SerializeField] private float BUTTON_RESIZE; // Tamaño para redimensaionar (el tamaño se suma al original)
    private PlayerInput playerInput;

    public UnityEvent<PlayerSlotManager> OnItemPressed; //When an item is totally selected (player clicked on the object)

    private PlayerSlotManager selectedSlot; //Current selected PlayerSlotmanager by the player
    public InventoryItem_ScriptableObject itemSO; //The itemSO contained in this PlayerSlotManager instance

    private bool IsDialogSelected; // When a option dialog is active on this instance, this is used to block any Deselect events

    private void OnEnable()
    {
        buttonObject.GetComponent<Button>();
        originalSize = this.buttonObject.transform.localScale;
        IsDialogSelected = false;
    }
    private void OnDisable()
    {

        selectedSlot = null;
        this.buttonObject.GetComponent<RectTransform>().localScale = originalSize;
    }

    private void Start()
    {
        originalSize = this.buttonObject.transform.localScale;
        playerInput = FindFirstObjectByType<PlayerInput>();
        FindFirstObjectByType<DialogManager>(FindObjectsInactive.Include).OnCreate.AddListener(this.OnDialogSelect);
        FindFirstObjectByType<DialogManager>(FindObjectsInactive.Include).OnClose.AddListener(this.OnDialogDeselect);
        foreach (PlayerSlotManager slot in FindObjectsByType<PlayerSlotManager>(FindObjectsSortMode.None))
            if (slot.gameObject != this.gameObject)
                slot.OnItemPressed.AddListener(ItemSelected);

    }

    private void ItemSelected(PlayerSlotManager otherSlot)
    {
        selectedSlot = otherSlot;
    }

    public InventoryItem_ScriptableObject SwitchItem(InventoryItem_ScriptableObject item)
    {
        InventoryItem_ScriptableObject oldSprite = this.itemSO;
        this.itemSO = item;
        if(item == null)
            this.itemObject.gameObject.SetActive(false);
        else {
            this.itemObject.sprite = item.ItemSprite;
            this.itemObject.gameObject.SetActive(true);
        }
        this.buttonObject.GetComponent<RectTransform>().localScale = originalSize;
        return oldSprite;
    }

    public void DropItem()
    {
        this.SwitchItem(null); // Remove icon
        OnItemPressed?.Invoke(null); // Reset selection
        this.buttonObject.GetComponent<RectTransform>().localScale = originalSize; // Revert sieze
    }

    /// <summary>
    /// This is for the DialogManager.OnCreate call
    /// </summary>
    private void OnDialogSelect(PlayerSlotManager slot){ if (slot == this) { IsDialogSelected = true; this.OnSelect(null); } }
    private void OnDialogDeselect(PlayerSlotManager slot) { if (slot == this) { IsDialogSelected = false; this.OnDeselect(null); } }

    public void OnSelect(BaseEventData eventData)
    {
        if (itemObject.gameObject.activeSelf)
        {
            this.buttonObject.GetComponent<RectTransform>().localScale = 
                new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
        }
        this.buttonObject.GetComponent<Image>().sprite = selectedSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (IsDialogSelected)
            return;
        this.buttonObject.GetComponent<Image>().sprite = normalSprite;
        if (itemObject.gameObject.activeSelf && this.selectedSlot != this)
        {
            this.buttonObject.GetComponent<RectTransform>().localScale = originalSize;
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        StartCoroutine(clickEffect());
    }
    IEnumerator clickEffect()
    {
        /* Click animation and input disable */
        this.buttonObject.GetComponent<Image>().sprite = pressedSprite;
        playerInput.currentActionMap.Disable();
        yield return new WaitForSeconds(0.1f);
        playerInput.currentActionMap.Enable();
        this.buttonObject.GetComponent<Image>().sprite = selectedSprite;

        /*  Item swap */
         
        if (!selectedSlot && this.itemObject.gameObject.activeSelf) // Event clicked item
        {
            OnItemPressed?.Invoke(this);
            selectedSlot = this;
            this.buttonObject.GetComponent<RectTransform>().localScale =
                new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
            this.buttonObject.GetComponent<Image>().sprite = pressedSprite;
        }
        else  if(selectedSlot)
        {

            InventoryItem_ScriptableObject viejo = selectedSlot.SwitchItem(this.itemSO);
            this.SwitchItem(viejo);
            this.selectedSlot = null;
            OnItemPressed?.Invoke(selectedSlot);
            this.OnSelect(null);
        }
            


    }
}
