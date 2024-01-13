using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionOptionMenuDialog : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
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

	

    private void OnDisable()
    {
		selected = false;
    }


	/*void Update()
	{
		if( selected && Input.GetKeyDown(KeyCode.Return))
		{
			dialogue.ManageResultChoiceDialog(this.gameObject.GetComponent<TextMeshProUGUI>().text);
			dialogMenu.SetActive(false);
		}
	}*/

    public void OnSubmit(BaseEventData eventData)
    {
        if (selected)
        {
            dialogue.ManageResultChoiceDialog(this.gameObject.GetComponent<TextMeshProUGUI>().text);
            dialogMenu.SetActive(false);
        }
    }
}

