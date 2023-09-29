using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSlotManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    private Button btn; // Referencia al botón del objeto
    private Vector3 originalSize; // Tamaño original del botón al comenzar el juego

    [SerializeField] private GameObject itemObject; // Item Object
    [SerializeField] private GameObject buttonObject; // Button Object
    [SerializeField] private Sprite selectedSprite; // Sprite usado al poner el ratón encima
    [SerializeField] private Sprite normalSprite; // Sprite cuando no está seleccionado
    [SerializeField] private float BUTTON_RESIZE; // Tamaño para redimensaionar (el tamaño se suma al original)

    private void Start()
    {
        this.btn = buttonObject.GetComponent<Button>();
        this.originalSize = this.btn.GetComponent<RectTransform>().localScale;
    }

    public void resizeItem()
    {
        this.btn.GetComponent<Image>().sprite = selectedSprite;
        if (this.hasItem())
            this.btn.GetComponent<RectTransform>().localScale = new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
    }

    public void revertSizeItem()
    {
        this.btn.GetComponent<Image>().sprite = normalSprite;
        this.btn.GetComponent<RectTransform>().localScale = originalSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        resizeItem();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        revertSizeItem();

    }

    public void addItem(Sprite item)
    {
        this.itemObject.SetActive(true);
        this.itemObject.GetComponent<Image>().sprite = item;
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
        revertSizeItem();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("A");

    }
}
