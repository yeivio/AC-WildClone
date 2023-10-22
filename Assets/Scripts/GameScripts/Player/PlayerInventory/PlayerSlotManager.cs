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
    [Header("Mandatory Assets")]
    [SerializeField] private Image itemObject; // Sprite of the item contained
    [SerializeField] private Button buttonObject; // The button Object
    [SerializeField] private Sprite selectedSprite; // Background Sprite when object is hovered by
    [SerializeField] private Sprite pressedSprite; // Sprite when an slot is clicked
    [SerializeField] private Sprite normalSprite; // Sprite when idle mode
    [SerializeField] private float BUTTON_RESIZE; // Tamaño para redimensaionar (el tamaño se suma al original)

    [Header("Events")]
    public UnityEvent<PlayerSlotManager> OnItemPressed; // When an item is totally selected (player clicked on the object)
    [Header("Public variables")]
    public InventoryItem_ScriptableObject itemSO; // The itemSO contained in this PlayerSlotManager instance
    public PlayerInventoryManager inventoryManager; // Reference to the inventoryManager

    private PlayerInput playerInput;
    private PlayerSlotManager selectedSlot; // Current selected PlayerSlotmanager by the player
    private Vector3 originalSize; // Original size
    private bool IsDialogSelected; // When a option dialog is active on this instance, this is used to block any OnDeselect events
    // This is because when opening the dialog box the selected button gets switched to the new one and the old one receives the OnDeselect

    private void OnEnable()
    {
        buttonObject.GetComponent<Button>();
        originalSize = this.buttonObject.transform.localScale;  // Save the original size
        IsDialogSelected = false;
    }
    private void OnDisable()
    {
        selectedSlot = null; // Forget the selectedSlot if there is one
        this.buttonObject.GetComponent<RectTransform>().localScale = originalSize; // Revert object to original size
    }

    private void Start()
    {
        originalSize = this.buttonObject.transform.localScale; // Save the original size
        playerInput = FindFirstObjectByType<PlayerInput>();
        FindFirstObjectByType<DialogManager>(FindObjectsInactive.Include).OnCreate.AddListener(this.OnDialogSelect); // Dialog is created event
        FindFirstObjectByType<DialogManager>(FindObjectsInactive.Include).OnClose.AddListener(this.OnDialogDeselect); // Dialog is closed event

        foreach (PlayerSlotManager slot in FindObjectsByType<PlayerSlotManager>(FindObjectsSortMode.None))
            if (slot.gameObject != this.gameObject)
                slot.OnItemPressed.AddListener(ItemSelected); // Event for when a player clicks on a PlayerSlot

    }

    /// <summary>
    /// Function linked to the OnItemPressed event for saving the current selected PlayerSlot object
    /// </summary>
    private void ItemSelected(PlayerSlotManager otherSlot)
    {
        selectedSlot = otherSlot;
    }

    /// <summary>
    /// Given an InventoryItem SO, switches the sprite between the old SO and the current one.
    /// If null is passed by argument, it will disable que sprite object
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The old InventoryItem SO</returns>
    public InventoryItem_ScriptableObject SwitchItem(InventoryItem_ScriptableObject item)
    {
        InventoryItem_ScriptableObject oldSprite = this.itemSO;
        this.itemSO = item;
        if(item == null)
            this.itemObject.gameObject.SetActive(false);    // Deactivate the sprite object (resets the sprite)
        else {
            this.itemObject.sprite = item.ItemSprite;
            this.itemObject.gameObject.SetActive(true);
        }
        this.buttonObject.GetComponent<RectTransform>().localScale = originalSize;  // Reverts the button size to the original
        return oldSprite;
    }

    /// <summary>
    /// Function for when a player wants to drop an item. Dropping an item disables the sprite and removes it from the SO
    /// </summary>
    public void DropItem(InventoryItem_ScriptableObject item)
    {
        this.SwitchItem(null); // Remove icon
        OnItemPressed?.Invoke(null); // Reset selection
        this.buttonObject.GetComponent<RectTransform>().localScale = originalSize; // Revert size
        this.inventoryManager.RemoveItem(item);
    }

    /// <summary>
    /// When DialogManager.OnCreate call occurs, if the 
    /// </summary>
    private void OnDialogSelect(PlayerSlotManager slot){ if (slot == this) { IsDialogSelected = true; this.OnSelect(null); } }
    private void OnDialogDeselect(PlayerSlotManager slot) { if (slot == this) { IsDialogSelected = false; this.OnDeselect(null); } }


    /// <summary>
    /// When a PlayerSlot is selected, the button changes its sprite color and resizes for visual feedback
    /// </summary>
    public void OnSelect(BaseEventData eventData)
    {
        if (itemObject.gameObject.activeSelf)
        {
            this.buttonObject.GetComponent<RectTransform>().localScale = 
                new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
        }
        this.buttonObject.GetComponent<Image>().sprite = selectedSprite;
    }

    /// <summary>
    /// When a PlayerSlot is deselected, the button changes its sprite color and resizes into it's original size.
    /// If the call is made when an DialogMenu is enabled with the PlayerSlot object, it will ignore it and won't do anything.
    /// This occurs because when switching into the DialogMenu.The PlayerSlot should keep it's size for feedback on which object 
    /// the DialogMenu is opened. When the DialogMenu is created, the new selected button is the one on the DialogMenu and 
    /// this function gets triggered.
    /// </summary>
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

    /// <summary>
    /// Couroutine for when a player clicks on a PlayerSlot. When this happens first it plays the click animation which consists
    /// of switching the sprite color of the object, and then, if there is not already a selectedSlot and the PlayerSlot contains 
    /// an item, triggers the event for the rest of PlayerSlots so they know there is an slot selected and resizes the current 
    /// slot. If the player clicks on an playerslot and there is already a selectedslot, they switch their items and resets the 
    /// selectedSlot for everyone.
    /// </summary>
    IEnumerator clickEffect()
    {
        /* Click animation and input disable */
        this.buttonObject.GetComponent<Image>().sprite = pressedSprite;
        playerInput.currentActionMap.Disable();
        yield return new WaitForSeconds(0.1f);
        playerInput.currentActionMap.Enable();
        this.buttonObject.GetComponent<Image>().sprite = selectedSprite;

        /*  Item swap */
         
        if (!selectedSlot && this.itemObject.gameObject.activeSelf) // Not selected and contains item
        {
            OnItemPressed?.Invoke(this);
            selectedSlot = this;
            this.buttonObject.GetComponent<RectTransform>().localScale =
                new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
            this.buttonObject.GetComponent<Image>().sprite = pressedSprite;
        }
        else  if(selectedSlot)  
        {
            // Switch items
            InventoryItem_ScriptableObject viejo = selectedSlot.SwitchItem(this.itemSO);
            this.SwitchItem(viejo);
            this.selectedSlot = null;
            OnItemPressed?.Invoke(selectedSlot);
            this.OnSelect(null);
        }
            


    }
}
