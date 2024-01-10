using UnityEngine;
using System.Collections;

public class SellButton : MonoBehaviour
{
    private SellMenu menuManger;
    public bool hasItem;
    public InventoryItem_ScriptableObject item;
    // Use this for initialization
    void Start()
    {
        menuManger = GameObject.FindAnyObjectByType<SellMenu>();
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

