using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSlotManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button btn; // Referencia al botón del objeto
    private Vector3 originalSize; // Tamaño original del botón al comenzar el juego

    [SerializeField] private GameObject itemObject; // Item Object
    [SerializeField] private GameObject buttonObject; // Button Object
    [SerializeField] private Sprite selectedSprite; // Sprite usado al poner el ratón encima
    [SerializeField] private Sprite pressedSprite; // Sprite usado cuando un objeto es seleccionado
    [SerializeField] private Sprite normalSprite; // Sprite cuando no está seleccionado
    [SerializeField] private float BUTTON_RESIZE; // Tamaño para redimensaionar (el tamaño se suma al original)

    public GameObject cursorPosition;

    private bool isSelected;

    private void OnEnable()
    {
        this.btn = buttonObject.GetComponent<Button>();
        this.originalSize = this.btn.GetComponent<RectTransform>().localScale;
        isSelected = false;
    }

    public void resizeItem()
    {
        if (!this.hasItem())
            return;
        this.btn.GetComponent<Image>().sprite = selectedSprite;
        this.btn.GetComponent<RectTransform>().localScale = new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
    }

    public void revertSizeItem()
    {
        this.btn.GetComponent<Image>().sprite = normalSprite;
        this.btn.GetComponent<RectTransform>().localScale = originalSize;
    }
    public void addItem(Sprite item)
    {
        this.itemObject.GetComponent<Image>().sprite = item;
        this.itemObject.SetActive(true);
        isSelected = false;
    }

    public Sprite removeItem()
    {
        this.itemObject.SetActive(false);
        return this.itemObject.GetComponent<Image>().sprite;
    }

    public bool hasItem()
    {
        return this.itemObject.activeSelf;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!this.selectedSprite)
            resizeItem();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(!isSelected)
            revertSizeItem();
    }

    public void OnClick()
    {
        if (!this.hasItem())
            return;
        this.isSelected = true;
        this.btn.GetComponent<Image>().sprite = pressedSprite;
        this.btn.GetComponent<RectTransform>().localScale = new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
    }

    public void UnClick()
    {
        this.isSelected = false;
        this.btn.GetComponent<Image>().sprite = normalSprite;
        this.btn.GetComponent<RectTransform>().localScale = originalSize;

    }
}
