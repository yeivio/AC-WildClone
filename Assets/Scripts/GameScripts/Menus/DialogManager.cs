using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dropItem_Text;
    [SerializeField] private GameObject closeMenu_Text;
    [SerializeField] private GameObject equipObject_Text;
    [SerializeField] private PlayerInteractionsController playerController;


    private PlayerSlotManager currentSelectedItem;

    private List<Button> listActiveButtons;

  

    private void OnDisable()
    {
        currentSelectedItem.DisableObjectName();
        this.equipObject_Text.SetActive(false);
    }

    /// <summary>
    /// Gets the active item properties and enables the possible options buttons
    /// </summary>
    private void OnEnable()
    {
        this.listActiveButtons = new List<Button>();
        currentSelectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>();
        currentSelectedItem.EnableObjectName();
        if (currentSelectedItem.GetItemSO().IsEquipable)
        {
            this.equipObject_Text.SetActive(true);
            listActiveButtons.Add(this.equipObject_Text.GetComponent<Button>());
        }
        else { 
            this.equipObject_Text.SetActive(false);
        }

        if (currentSelectedItem.GetItemSO().IsDropeable)
        {
            this.dropItem_Text.SetActive(true);
            listActiveButtons.Add(this.dropItem_Text.GetComponent<Button>());
        }
        else
        {
            this.dropItem_Text.SetActive(false);
        }



        listActiveButtons.Add(this.closeMenu_Text.GetComponent<Button>());
        this.closeMenu_Text.gameObject.GetComponent<Button>().Select();
        this.buildNavigation();
    }

    public void DropItem()
    {
        PlayerInteractionsController playerInteraction = FindAnyObjectByType<PlayerInteractionsController>();

        InventoryItem_ScriptableObject delItem = currentSelectedItem.GetItemSO();
        //Debug.Log($"Building system: {BuildingSystem.current}");
        //Debug.Log($"Item {delItem}");
        //Debug.Log($"Object3D: {delItem.object3D}");
        PlayerController player = FindAnyObjectByType<PlayerController>();
        BuildingSystem buildSys = BuildingSystem.current;
        bool droped = buildSys.DropItem(
            delItem.object3D,
            player.gameObject,
            buildSys.LookingDirection(player.gameObject));
        if (droped)
        {
            // TODO only delete the visual item if the item is being droped
            // Possible solution drop the item on the position of the player

            //Checks if the object is equipped 
            if (playerInteraction.getEquippedItem() == delItem)
                playerInteraction.UnequipItem();
            currentSelectedItem.removeItemSO();
            this.CloseMenu();
        }
        
    }

    public void EquipItem()
    {
        playerController.EquipItem(this.currentSelectedItem.GetItemSO());
    }

    public void CloseMenu()
    {
        currentSelectedItem.DisableObjectName();
        currentSelectedItem.gameObject.GetComponent<Button>().Select();
        this.gameObject.SetActive(false);
    }

    private void buildNavigation()
    {
        Navigation customNav;
        customNav = new Navigation();
        customNav.mode = Navigation.Mode.Explicit;

        for (int i = 0; i < listActiveButtons.Count; i++)
        {
            customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            int previousIndex = (i - 1 + listActiveButtons.Count) % listActiveButtons.Count;
            int nextIndex = (i + 1) % listActiveButtons.Count;

            customNav.selectOnUp = listActiveButtons[previousIndex];
            customNav.selectOnDown = listActiveButtons[nextIndex];

            listActiveButtons[i].gameObject.GetComponent<Button>().navigation = customNav;
        }
    }



}
