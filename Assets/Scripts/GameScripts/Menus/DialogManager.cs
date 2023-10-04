using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Button firstSelectedButton;

    public UnityEvent OnClose;
    public UnityEvent OnItemDrop;

    private void OnEnable()
    {
        this.firstSelectedButton.Select();

    }

    public void DropItem()
    {
        this.gameObject.SetActive(false);
        this.OnItemDrop?.Invoke();
        this.OnClose?.Invoke();
    }



}
