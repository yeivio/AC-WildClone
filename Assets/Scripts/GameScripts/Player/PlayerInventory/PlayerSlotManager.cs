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

    public UnityEvent<PlayerSlotManager> OnItemPressed;

    private PlayerSlotManager selectedSlot;


    private void Start()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        foreach (PlayerSlotManager slot in FindObjectsByType<PlayerSlotManager>(FindObjectsSortMode.None))
            if (slot.gameObject != this.gameObject)
                slot.OnItemPressed.AddListener(ItemSelected);
    }

    private void ItemSelected(PlayerSlotManager otherSlot)
    {
        selectedSlot = otherSlot;
    }

    public Sprite SwitchItem(Sprite item)
    {
        Sprite oldSprite = this.itemObject.sprite;
        this.itemObject.sprite = item;
        if(this.itemObject.sprite == null)
            this.itemObject.gameObject.SetActive(false);
        else
            this.itemObject.gameObject.SetActive(true);
        return oldSprite;
    }

    private void OnEnable()
    {
        buttonObject.GetComponent<Button>();
        originalSize = this.buttonObject.transform.localScale;
    }
    private void OnDisable()
    {
        itemObject.sprite = null;
        selectedSlot = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (itemObject.sprite)
        {
            this.buttonObject.GetComponent<RectTransform>().localScale = 
                new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
        }
        this.buttonObject.GetComponent<Image>().sprite = selectedSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        this.buttonObject.GetComponent<Image>().sprite = normalSprite;
        if (itemObject.sprite)
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
         
        if (!selectedSlot && this.itemObject.sprite) // Event clicked item
            OnItemPressed?.Invoke(this);
        else  if(selectedSlot)
        {
            Sprite viejo = selectedSlot.SwitchItem(this.itemObject.sprite);
            this.SwitchItem(viejo);
            this.selectedSlot = null;
            OnItemPressed?.Invoke(selectedSlot);
            this.OnSelect(null);
        }
            


    }
}
