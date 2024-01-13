using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerInputController : MonoBehaviour
{

    public InputActionAsset inputAction;
    public PlayerInput playerInput;

    public PlayerController playerController;
    public PlayerInventoryManager inventoryManager;
    public PlayerInteractionsController interactionController;
    public FishingObject fishingObj;
    public FishingUI_Manager fishingUI;
    public Npc_Dialogue npcdialog;

    public UI_Icon_Manager iconManager;
    public BuyMenu buyMenu;
    public SellMenu sellMenu;
    public OptionsManager optionsManager;

    public InputSystemUIInputModule Navigation_UI;
    public InputSystemUIInputModule Navigation_NPCTALK;

    public UI_Icon_Manager.ControllerType activeController; // This is for the UI_ICON_Manager 

    private void Start()
    { 

        //FREEMOVE ACTIONS 
        this.inputAction.FindAction(Utils.FREEMOVE_INVENTORY).performed += this.inventoryManager.OpenInventory;
        this.inputAction.FindAction(Utils.FREEMOVE_INTERACT).performed += this.interactionController.Interact;
        this.inputAction.FindAction(Utils.FREEMOVE_UNEQUIP_ITEM).performed += this.interactionController.UnequipItem;
        this.inputAction.FindAction(Utils.FREEMOVE_UNEQUIP_DIG).performed += this.interactionController.PlayerDig;

        //UI ACTIONS
        this.inputAction.FindAction(Utils.UI_INPUTMAP_CLOSEMENU).performed += this.inventoryManager.CloseMenu;
        this.inputAction.FindAction(Utils.UI_INPUTMAP_CLOSEMENU).performed += this.npcdialog.CloseDialog;
        this.inputAction.FindAction(Utils.UI_INPUTMAP_ALTERNATIVECONFIRM).performed += this.inventoryManager.OpenDialogWindow;

        //MOVING OBJECTS ACTIONS
        this.inputAction.FindAction(Utils.MOVINGOBJECTS_DIRECTION).performed += this.interactionController.MoveObject;
        this.inputAction.FindAction(Utils.MOVINGOBJECTS_EXIT).performed += this.interactionController.ExitMoveObject;

        //FISHING ACTIONS
        this.inputAction.FindAction(Utils.FISHING_INPUTMAP_ACTION).performed += this.fishingUI.action;
        this.inputAction.FindAction(Utils.FISHING_INPUTMAP_QUIT).performed += this.fishingObj.stopInteraction;

        //NPC_TALK ACTIONS
        this.inputAction.FindAction(Utils.NPC_TALK_QUIT).performed += this.npcdialog.CloseDialog;
        this.inputAction.FindAction(Utils.NPC_TALK_CONTINUE).performed += this.npcdialog.ContinueConversation;
        this.inputAction.FindAction(Utils.NPC_TALK_CONFIRM).performed += this.optionsManager.Accept;

        //NPC_TALK BUY
        this.inputAction.FindAction(Utils.NPC_TALK_BUY_CONFIRM).performed += this.buyMenu.Confirm;
        this.inputAction.FindAction(Utils.NPC_TALK_BUY_QUIT).performed += this.npcdialog.CloseDialog;

        //NPC_TALK SELL
        this.inputAction.FindAction(Utils.NPC_TALK_SELL_SELECT).performed += this.sellMenu.Select;
        this.inputAction.FindAction(Utils.NPC_TALK_SELL_CONFIRM).performed += this.sellMenu.Confirm;
        this.inputAction.FindAction(Utils.NPC_TALK_SELL_QUIT).performed += this.npcdialog.CloseDialog;

    }
    public void SwitchInputMap(string newMap)
    {
        playerInput.SwitchCurrentActionMap(newMap);
        if(newMap == Utils.NPC_TALK_INPUTMAP)
        {
            Navigation_UI.enabled = false;
            Navigation_NPCTALK.enabled = true;
        }
        if (newMap == Utils.UI_INPUTMAP)
        {
            Navigation_UI.enabled = true;
            Navigation_NPCTALK.enabled = false;
        }
    }

    /// <summary>
    /// Detects the current device the player is using and switches the UI Icons.
    /// </summary>
    /// <param name="input"></param>
    public void ControlChange(PlayerInput input)
    {
        if(input.currentControlScheme == input.defaultControlScheme)
            activeController = UI_Icon_Manager.ControllerType.KEYBOARD;
        else
            activeController = UI_Icon_Manager.ControllerType.XBOX;
    }

}
