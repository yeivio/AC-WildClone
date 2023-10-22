using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dropItem_Text;
    [SerializeField] private GameObject closeMenu_Text;
    [SerializeField] private GameObject eatObject_Text;
    [SerializeField] private GameObject plant_Text;

    private PlayerSlotManager currentSelectedItem;

    public UnityEvent<PlayerSlotManager> OnClose;
    public UnityEvent<PlayerSlotManager> OnCreate;

    private void OnDisable()
    {
        this.eatObject_Text.SetActive(false);
        this.plant_Text.SetActive(false);
    }

    private void OnEnable()
    {
        currentSelectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<PlayerSlotManager>();
        OnCreate?.Invoke(currentSelectedItem);
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

        Vector3 position = FindAnyObjectByType<PlayerController>().transform.position;
        InventoryItem_ScriptableObject delItem = currentSelectedItem.itemSO;
        //Debug.Log($"Building system: {BuildingSystem.current}");
        //Debug.Log($"Item {delItem}");
        //Debug.Log($"Object3D: {delItem.object3D}");

        bool droped = BuildingSystem.current.DropItem(delItem.object3D, position);
        if (droped)
        {
            // TODO only delete the visual item if the item is being droped
            // Possible solution drop the item on the position of the player
            currentSelectedItem.DropItem(currentSelectedItem.itemSO);
            this.CloseMenu();
        }
        
    }

    public void CloseMenu()
    {
        this.OnClose?.Invoke(currentSelectedItem);
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
