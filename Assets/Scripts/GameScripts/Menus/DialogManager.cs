using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Button firstSelectedButton;
    [SerializeField] private GameObject dropItem_Text;
    [SerializeField] private GameObject closeMenu_Text;
    [SerializeField] private GameObject eatObject_Text;
    [SerializeField] private GameObject plant_Text;

    private PlayerSlotManager currentSelectedItem;

    public UnityEvent OnClose;
    public UnityEvent<InventoryItem_ScriptableObject> OnItemDrop;

    private void OnDisable()
    {
        this.eatObject_Text.SetActive(false);
        this.plant_Text.SetActive(false);
    }

    private void OnEnable()
    {
        currentSelectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>();
        this.firstSelectedButton.Select();

        if (currentSelectedItem.itemSO.IsEatable) { 
            this.eatObject_Text.SetActive(true);
        }
        if (currentSelectedItem.itemSO.IsPlantable)
            this.plant_Text.SetActive(true);
    }

    public void DropItem()
    {
        currentSelectedItem.DropItem();
        this.OnItemDrop?.Invoke(currentSelectedItem.itemSO);
        this.OnClose?.Invoke();
        this.gameObject.SetActive(false);
    }



}
