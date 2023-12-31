﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BuyMenu : MonoBehaviour
{
    private GraphicRaycaster rc;
    private EventSystem eventSystem;
    private PointerEventData pt;
    [SerializeField] private Button first;
    [SerializeField] private GameObject menu;                       // element to be disable when quiting the menu
    public BuyButton selected;
    [SerializeField] private BuyButton buying;                      // element that is being bought
    [SerializeField] private TalkableObject npc;                    // NPC that calls the menu

    private Button [] shellingElements;
    public List<InventoryItem_ScriptableObject> items;

    // Use this for initialization
    void Start()
	{
        rc = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        List<Button> buttonsAux = new();
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("BuySlot"))
        {
            //Debug.Log(b);
            //Debug.Log(b.GetComponent<Button>());

            buttonsAux.Add(b.GetComponent<Button>());
        }
        shellingElements = buttonsAux.ToArray();
        foreach (InventoryItem_ScriptableObject item in items)
        {
            int index = items.IndexOf(item);
            shellingElements[index].gameObject.GetComponent<Image>().sprite = item.ItemSprite;
            shellingElements[index].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = item.BuyPrice.ToString();
            shellingElements[index].gameObject.GetComponent<BuyButton>().hasItem = true;
            shellingElements[index].gameObject.GetComponent<BuyButton>().item = item;
        }
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
        if(EventSystem.current.currentSelectedGameObject.TryGetComponent<Button>(out Button but))
            selected = but.gameObject.GetComponent<BuyButton>();

    }
    private void OnEnable()
    {
        first.Select();
    }
    public void setBuying(BuyButton buying)
    {
        this.buying = buying;
        //quitText.dialogue += buying.gameObject.GetComponent<TextMeshProUGUI>().text + " bayas.";
        
        npc.Continue(" "+buying.item.BuyPrice.ToString() + " bayas.");
        menu.SetActive(false);
    }
    public BuyButton getBuying()
    {
        return buying;
    }
}

