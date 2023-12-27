using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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


    private void Start()
    { 

        //FREEMOVE ACTIONS 
        this.inputAction.FindAction(Utils.FREEMOVE_INVENTORY).performed += this.inventoryManager.OpenInventory;
        this.inputAction.FindAction(Utils.FREEMOVE_INTERACT).performed += this.interactionController.Interact;
        this.inputAction.FindAction(Utils.FREEMOVE_UNEQUIP_ITEM).performed += this.interactionController.UnequipItem;

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

    }

    public void SwitchInputMap(string newMap)
    {
        playerInput.SwitchCurrentActionMap(newMap);
    }

    public void ControlChange(PlayerInput input)
    {
        if(input.currentControlScheme == input.defaultControlScheme)
            UI_Icon_Manager.activeController = UI_Icon_Manager.ControllerType.KEYBOARD;
        else
            UI_Icon_Manager.activeController = UI_Icon_Manager.ControllerType.XBOX;
    }

}
