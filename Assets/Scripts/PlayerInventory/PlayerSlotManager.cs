using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSlotManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button btn; // Referencia al bot�n del objeto
    private Vector3 originalSize; // Tama�o original del bot�n al comenzar el juego

    [SerializeField] private GameObject itemObject; // Item Object
    [SerializeField] private GameObject buttonObject; // Button Object
    [SerializeField] private Sprite selectedSprite; // Sprite usado al poner el rat�n encima
    [SerializeField] private Sprite pressedSprite; // Sprite usado cuando un objeto es seleccionado
    [SerializeField] private Sprite normalSprite; // Sprite cuando no est� seleccionado
    [SerializeField] private float BUTTON_RESIZE; // Tama�o para redimensaionar (el tama�o se suma al original)
    private bool isSelected;

    private void Start()
    {
        this.btn = buttonObject.GetComponent<Button>();
        this.originalSize = this.btn.GetComponent<RectTransform>().localScale;
    }
    private void OnDisable()
    {
        this.GetComponent<Image>().sprite = normalSprite;
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
        UnClick();
    }
    public void addItem(Sprite item)
    {
        this.itemObject.SetActive(true);
        this.itemObject.GetComponent<Image>().sprite = item;
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
    public Sprite getItem()
    {
        return this.itemObject.GetComponent<Image>().sprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
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
