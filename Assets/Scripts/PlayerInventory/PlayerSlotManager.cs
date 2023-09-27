using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSlotManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.btn.GetComponent<Image>().sprite = selectedSprite;
        if(this.hasItem())
            this.btn.GetComponent<RectTransform>().localScale = new Vector3(originalSize.x + BUTTON_RESIZE, originalSize.y + BUTTON_RESIZE, originalSize.z);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        this.btn.GetComponent<Image>().sprite = normalSprite;
        this.btn.GetComponent<RectTransform>().localScale = originalSize;

    }

    public void addItem(Sprite item)
    {
        this.itemObject.SetActive(true);
        this.itemObject.GetComponent<Image>().sprite = item;
    }

    public void removeItem()
    {
        this.itemObject.SetActive(false);
    }

    public bool hasItem()
    {
        return this.itemObject.activeSelf;
    }

}
