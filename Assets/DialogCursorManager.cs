using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogCursorManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject cursor;
    private void OnDisable()
    {
        this.cursor.gameObject.SetActive(false);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        this.cursor.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.cursor.gameObject.SetActive(true);
    }
}
