using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuyMenu : MonoBehaviour
{
    private GraphicRaycaster rc;
    private EventSystem eventSystem;
    private PointerEventData pt;
    [SerializeField] private Button first;
    public BuyButton selected;
    public BuyButton buying;

    private List<Button> shellingElements;

    // Use this for initialization
    void Start()
	{
        rc = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        shellingElements = new();

        foreach (GameObject b in GameObject.FindGameObjectsWithTag("BuySlot"))
        {
            Debug.Log(b);
            Debug.Log(b.GetComponent<Button>());
            shellingElements.Add(b.GetComponent<Button>());
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
}

