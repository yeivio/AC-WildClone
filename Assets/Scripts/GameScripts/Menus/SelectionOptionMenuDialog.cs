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

    private bool canRecieveinput = false;

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
		selected = false;
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
		selected = true;
    }

	

    private void OnDisable()
    {
		selected = false;
        canRecieveinput = false;
    }


    /*void Update()
	{
		if( selected && Input.GetKeyDown(KeyCode.Return))
		{
			dialogue.ManageResultChoiceDialog(this.gameObject.GetComponent<TextMeshProUGUI>().text);
			dialogMenu.SetActive(false);
		}
	}*/

    public void Submit()
    {        
        if (gameObject.activeSelf && selected && canRecieveinput)
        {
            dialogue.ManageResultChoiceDialog(this.gameObject.GetComponent<TextMeshProUGUI>().text);
            dialogMenu.SetActive(false);
        }
    }

    public void permitirInput()
    {
        canRecieveinput = true;
    }
}

