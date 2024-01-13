using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SellButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private SellMenu menuManger;
    public bool hasItem;
    public InventoryItem_ScriptableObject item;
    public TextMeshProUGUI itemName;

    // Use this for initialization
    void Start()
    {
        menuManger = GameObject.FindAnyObjectByType<SellMenu>();
        foreach (Transform child in transform) //Activate text and backgrounds
        {
            if (child.gameObject.CompareTag("NameInfo"))
                itemName = child.gameObject.GetComponent<TextMeshProUGUI>();
        }
    }
    private void OnEnable()
    {
        menuManger = GameObject.FindAnyObjectByType<SellMenu>();
        foreach (Transform child in transform) //Activate text and backgrounds
        {
            if (child.gameObject.CompareTag("NameInfo"))
                itemName = child.gameObject.GetComponent<TextMeshProUGUI>();
        }
    }
    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        if (hasItem)
            itemName.text = "";
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        if (hasItem)
            itemName.text = item.Name;
    }

    public void SelectItem()
    {
        bool isAdded = menuManger.SetSelling(this);

        foreach (Transform child in transform) //Activate text and backgrounds
        {
            Color32 color = new Color32(148, 255, 162, 255);
            if (!isAdded)
                color = new Color32(255, 255, 255, 255);

            if (!child.gameObject.CompareTag("NameInfo"))
                child.gameObject.GetComponent<TextMeshProUGUI>().color = color;
        }
    }
}

