using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using static UnityEditor.Progress;

public class SellMenu : MonoBehaviour
{
    private GraphicRaycaster rc;
    private EventSystem eventSystem;
    private PointerEventData pt;
    [SerializeField] private Button first;
    [SerializeField] private GameObject menu;                       // element to be disable when quiting the menu
    public Button selected;
    [SerializeField] private List<SellButton> shelling = new();                      // element that is being bought
    [SerializeField] private TalkableObject npc;                    // NPC that calls the menu

    private Button[] shellingElements;                              // Buttons that represent each item to be sold by the player 
    public PlayerInventory_ScriptableObject inventory;              // Player Inventory
    private InventoryItem_ScriptableObject[] inventoryItems;        // Inventory Items
    public int price;                                               // Price of the element to be selled

    private int MAX_SIZE = 12;                                       // Max number of elements in a page of the sell menu
    private int page;                                               // Actual page where user is

    [SerializeField] private Sprite initialImage;

    // Use this for initialization
    void Start()
    {
        rc = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        pt = new PointerEventData(eventSystem);
        pt.position = Input.mousePosition;
        List<RaycastResult> results = new();
        rc.Raycast(pt, results);

        foreach (RaycastResult s in results)
        {

            Debug.Log(s);
            if (s.gameObject.TryGetComponent<Button>(out Button b))
            {
                b.Select();
               
            }

                
        }
        */
        if (EventSystem.current.currentSelectedGameObject.TryGetComponent<Button>(out Button but))
            selected = but;

        if (Input.GetKeyDown(KeyCode.Return))
            SellAction();
    }
    private void OnEnable()
    {
        List<Button> buttonsAux = new();
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("SellSlot"))
        {
            //Debug.Log(b);
            //Debug.Log(b.GetComponent<Button>());

            buttonsAux.Add(b.GetComponent<Button>());
        }
        shellingElements = buttonsAux.ToArray();

        List<InventoryItem_ScriptableObject> items = new();

        foreach(InventoryItem_ScriptableObject item in inventory.getList())
        {
            if (item.IsSalable)
                items.Add(item);
        }

        inventoryItems = items.ToArray();
        int count = 0;
        page = 0;
        foreach (InventoryItem_ScriptableObject item in items)
        {
            if (count >= MAX_SIZE)
                break;
            modifyShellButtonInfo(
                   count,
                   item.ItemSprite,
                   item.SellPrice.ToString(),
                   item);
            count += 1;
        }
        first.Select();
    }
    public void NextPage()
    {
        if (page == 2)
            page = 0;
        else
            page += 1;
        int ini = page * MAX_SIZE;
        for (int i = 0; i<=(MAX_SIZE-1); i++)
        {
            int index = i + ini;

            if (inventoryItems.Length <= index)
            {
                modifyShellButtonInfo(
                   i,
                   initialImage,
                   "",
                   null);
            }
            else
            {
                modifyShellButtonInfo(
                   i,
                   inventoryItems[index].ItemSprite,
                   inventoryItems[index].SellPrice.ToString(),
                   inventoryItems[index]);
            }
            
        }
        
    }
    public void PreviousPage()
    {
        if (page == 0)
            page = 2;
        else
            page -= 1;
        int ini = page * MAX_SIZE;
        for (int i = 0; i <= (MAX_SIZE - 1); i++)
        {
            int index = i + ini;
            if (inventoryItems.Length <= index)
            {
                modifyShellButtonInfo(
                   i,
                   initialImage,
                   "",
                   null);
            }
            else
            {
                modifyShellButtonInfo(
                    i,
                    inventoryItems[index].ItemSprite,
                    inventoryItems[index].SellPrice.ToString(),
                    inventoryItems[index]);
            }
        }
    }
    private void modifyShellButtonInfo(int index, Sprite sprite, string price, InventoryItem_ScriptableObject item)
    {
        this.shellingElements[index].gameObject.GetComponent<Image>().sprite = sprite;
        SellButton actualButton = shellingElements[index].gameObject.GetComponent<SellButton>();
        TextMeshProUGUI itemPrice = new();
        foreach (Transform child in actualButton.transform) //Activate text and backgrounds
        {
            if (!child.gameObject.CompareTag("NameInfo"))
                itemPrice = child.gameObject.GetComponent<TextMeshProUGUI>();
        }
        itemPrice.text = price;
        actualButton.hasItem = true;
        actualButton.item = item;
    }
    public void SetSelling(SellButton toSell)
    {
        if (shelling.Contains(toSell))
            return;
        shelling.Add(toSell) ;
    }
    public void SellAction()
    {
        price = 0;
        foreach (SellButton sellingItem in shelling)
        {
            price += sellingItem.item.SellPrice;
        }
        npc.Continue(" " + price.ToString() + " bayas.");
        menu.SetActive(false);
    }
    public List<SellButton> GetSell()
    {
        return shelling;
    }
}

