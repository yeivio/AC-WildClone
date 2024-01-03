using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
	private BuyMenu menuManger;
	// Use this for initialization
	void Start()
	{
		menuManger = GameObject.FindAnyObjectByType<BuyMenu>();
	}

	// Update is called once per frame
	void Update()
	{
		if (menuManger.selected == this && Input.GetKeyDown(KeyCode.Return))
        {
			menuManger.buying = this;

        }
    }
}

