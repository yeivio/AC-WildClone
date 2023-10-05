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

        if (currentSelectedItem.itemSO.IsPlantable)
            this.plant_Text.SetActive(true);
        else
            this.plant_Text.SetActive(false);

        if (currentSelectedItem.itemSO.IsEatable)
            this.eatObject_Text.SetActive(true);
        else
            this.eatObject_Text.SetActive(false);

        this.closeMenu_Text.gameObject.GetComponent<Button>().Select();
        this.buildNavigation();
    }

    public void DropItem()
    {
        currentSelectedItem.DropItem();
        this.OnItemDrop?.Invoke(currentSelectedItem.itemSO);
        this.CloseMenu();
    }

    public void CloseMenu()
    {
        this.OnClose?.Invoke();
        this.gameObject.SetActive(false);
    }

    private void buildNavigation()
    {
        Navigation customNav;
        if (this.eatObject_Text.activeSelf && this.plant_Text.activeSelf) // Both actives
        {
            // Eat Navigation
            customNav = new Navigation(); 
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnDown = plant_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = closeMenu_Text.gameObject.GetComponent<Button>();
            eatObject_Text.gameObject.GetComponent<Button>().navigation = customNav;

            // Plant navigation

            customNav.selectOnDown = eatObject_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = dropItem_Text.gameObject.GetComponent<Button>();
            plant_Text.gameObject.GetComponent<Button>().navigation = customNav;


            // Drop navigation
            customNav.selectOnDown = closeMenu_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = plant_Text.gameObject.GetComponent<Button>();
            dropItem_Text.gameObject.GetComponent<Button>().navigation = customNav;

            // Close navigation
            customNav.selectOnDown = closeMenu_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = dropItem_Text.gameObject.GetComponent<Button>();
            closeMenu_Text.gameObject.GetComponent<Button>().navigation = customNav;

        }

        else if (this.eatObject_Text.activeSelf && !this.plant_Text.activeSelf) // Only eatObject
        {
            // Eat Navigation
            customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnDown = dropItem_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = closeMenu_Text.gameObject.GetComponent<Button>();
            eatObject_Text.gameObject.GetComponent<Button>().navigation = customNav;

            // Drop navigation
            customNav.selectOnDown = closeMenu_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = eatObject_Text.gameObject.GetComponent<Button>();
            dropItem_Text.gameObject.GetComponent<Button>().navigation = customNav;

            // Close navigation
            customNav.selectOnDown = eatObject_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = dropItem_Text.gameObject.GetComponent<Button>();
            closeMenu_Text.gameObject.GetComponent<Button>().navigation = customNav;


        }

        else if (!this.eatObject_Text.activeSelf && this.plant_Text.activeSelf) // Only plant
        {
            // Plant Navigation
            customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnDown = dropItem_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = closeMenu_Text.gameObject.GetComponent<Button>();
            plant_Text.gameObject.GetComponent<Button>().navigation = customNav;

            // Drop navigation
            customNav.selectOnDown = closeMenu_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = plant_Text.gameObject.GetComponent<Button>();
            dropItem_Text.gameObject.GetComponent<Button>().navigation = customNav;

            // Close navigation
            customNav.selectOnDown = plant_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = dropItem_Text.gameObject.GetComponent<Button>();
            closeMenu_Text.gameObject.GetComponent<Button>().navigation = customNav;


        }
        else
        {
            // Drop navigation
            customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnDown = closeMenu_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = closeMenu_Text.gameObject.GetComponent<Button>();
            dropItem_Text.gameObject.GetComponent<Button>().navigation = customNav;

            // Close navigation
            customNav.selectOnDown = dropItem_Text.gameObject.GetComponent<Button>();
            customNav.selectOnUp = dropItem_Text.gameObject.GetComponent<Button>();
            closeMenu_Text.gameObject.GetComponent<Button>().navigation = customNav;

        }

    }



}
