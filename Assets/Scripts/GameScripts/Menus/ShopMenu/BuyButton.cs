using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	private BuyMenu menuManger;
	public bool hasItem;
	public InventoryItem_ScriptableObject item;
	public TextMeshProUGUI itemName;
	// Use this for initialization
	void Start()
	{
		menuManger = GameObject.FindAnyObjectByType<BuyMenu>();
        foreach (Transform child in transform) //Activate text and backgrounds
        {
			if (child.gameObject.CompareTag("NameInfo"))
				itemName = child.gameObject.GetComponent<TextMeshProUGUI>();
        }
        
	}
    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        if(hasItem)
            itemName.text = "";
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        if(hasItem)
            itemName.text = item.Name;
    }

 
}

