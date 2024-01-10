using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionOptionMenuDialog : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	public GameObject dialogMenu;
	public Npc_Dialogue dialogue;
	private bool selected = false;

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
		selected = false;
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
		selected = true;
    }

	

    // Use this for initialization
    void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if( selected && Input.GetKeyDown(KeyCode.Return))
		{
			dialogue.ManageResultChoiceDialog(this.gameObject.GetComponent<TextMeshProUGUI>().text);
			dialogMenu.SetActive(false);
		}
	}

	
}

