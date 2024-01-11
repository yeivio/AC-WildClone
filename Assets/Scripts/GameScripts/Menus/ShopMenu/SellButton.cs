using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;

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
            itemName.text = item.name;
    }
    // Update is called once per frame
    void Update()
    {
        if (menuManger.selected == this && Input.GetKeyDown(KeyCode.Space) && hasItem)
        {
            menuManger.SetSelling(this);

        }
    }
}

