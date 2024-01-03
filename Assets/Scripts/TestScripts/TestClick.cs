using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TestClick : MonoBehaviour
{
	private GraphicRaycaster rc;
	private EventSystem eventSystem;
	private PointerEventData pt;
    // Use this for initialization
    void Start()
	{
		rc = GetComponent<GraphicRaycaster>();
		eventSystem = GetComponent<EventSystem>();
	}

	// Update is called once per frame
	void Update()
	{
		pt = new PointerEventData(eventSystem);
		pt.position = Input.mousePosition;
		List<RaycastResult> results = new();
		rc.Raycast(pt, results);

		foreach (RaycastResult s in results)
        {

            Debug.Log(s);
		}
	}
}

