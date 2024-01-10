using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
	private BuyMenu menuManger;
	public bool hasItem;
	public InventoryItem_ScriptableObject item;
	// Use this for initialization
	void Start()
	{
		menuManger = GameObject.FindAnyObjectByType<BuyMenu>();
	}

	// Update is called once per frame
	void Update()
	{
		if (menuManger.selected == this && Input.GetKeyDown(KeyCode.Space) && hasItem)
        {
			menuManger.setBuying(this);

        }
    }
}

