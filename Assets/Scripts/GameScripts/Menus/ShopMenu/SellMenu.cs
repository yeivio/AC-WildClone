using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SellMenu : MonoBehaviour
{
    private GraphicRaycaster rc;
    private EventSystem eventSystem;
    private PointerEventData pt;
    [SerializeField] private Button first;
    [SerializeField] private GameObject menu;                       // element to be disable when quiting the menu
    public SellButton selected;
    [SerializeField] private List<SellButton> shelling = new();                      // element that is being bought
    [SerializeField] private TalkableObject npc;                    // NPC that calls the menu

    private Button[] shellingElements;                              // Buttons that represent each item to be sold by the player 
    public PlayerInventory_ScriptableObject inventory;              // Player Inventory
    public int price;                                               // Price of the element to be selled

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
            selected = but.gameObject.GetComponent<SellButton>();

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

        List<InventoryItem_ScriptableObject> items = inventory.getList();
        int count = 0;
        foreach (InventoryItem_ScriptableObject item in items)
        {
            if (count >= this.shellingElements.Length)
                break;
            this.shellingElements[count].gameObject.GetComponent<Image>().sprite = item.ItemSprite;
            this.shellingElements[count].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = item.SellPrice.ToString();
            this.shellingElements[count].gameObject.GetComponent<SellButton>().hasItem = true;
            this.shellingElements[count].gameObject.GetComponent<SellButton>().item = item;
            count += 1;
        }
        first.Select();
    }
    public void SetSelling(SellButton toSell)
    {
        shelling.Add(toSell) ;
        //quitText.dialogue += buying.gameObject.GetComponent<TextMeshProUGUI>().text + " bayas.";

        
    }
    public void SellAction()
    {
        Debug.Log("SELL ACTION");
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

