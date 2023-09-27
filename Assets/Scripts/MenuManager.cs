using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject playerInventory; //Main player inventory
    [SerializeField] private GameObject playerInventory_optionDialog; //Dialog for action confirmation

    private GameObject currentMenu; //Current open menu
    private GameObject lastMenu; //Last opened menu

    

    
    public void OpenPlayerInventory()
    {
        if (currentMenu != null)
            this.lastMenu = currentMenu; 
        
        playerInventory.SetActive(true);
        currentMenu = playerInventory;
    }

    public void OpenDialogConfirmation()
    {
        if (currentMenu != null)
            this.lastMenu = currentMenu;
        playerInventory_optionDialog.SetActive(true);
        currentMenu = playerInventory_optionDialog;
    }

    /// <summary>
    /// Method for disabling the current active menu on screen
    /// </summary>
    /// <returns>True if main menu was closed and false if the main menu wasn't the last one closed </returns>
    public bool CloseMenu()
    {
        if (currentMenu == playerInventory) {
            
            currentMenu.SetActive(false);
            return true;
        }
        else
        {
            currentMenu = lastMenu;
            currentMenu.SetActive(false);
            return false;
        }
            
        
            
    }

}
