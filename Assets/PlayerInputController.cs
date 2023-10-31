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


    private void Start()
    { 
        //FREEMOVE ACTIONS 
        this.inputAction.FindAction(Utils.FREEMOVE_INVENTORY).performed += this.inventoryManager.OpenInventory;
        this.inputAction.FindAction(Utils.FREEMOVE_INTERACT).performed += this.interactionController.Interact;


        //UI ACTIONS
        this.inputAction.FindAction(Utils.UI_INPUTMAP_CLOSEMENU).performed += this.inventoryManager.CloseMenu;
        this.inputAction.FindAction(Utils.UI_INPUTMAP_ALTERNATIVECONFIRM).performed += this.inventoryManager.OpenDialogWindow;

        //MOVING OBJECTS ACTIONS
        this.inputAction.FindAction(Utils.MOVINGOBJECTS_DIRECTION).performed += this.interactionController.MoveObject;
        this.inputAction.FindAction(Utils.MOVINGOBJECTS_EXIT).performed += this.interactionController.ExitMoveObject;

        //FISHING ACTIONS
        this.inputAction.FindAction(Utils.FISHING_INPUTMAP_ACTION).performed += this.fishingUI.action;
        this.inputAction.FindAction(Utils.FISHING_INPUTMAP_QUIT).performed += this.fishingObj.stopInteraction;
    }

    public void SwitchInputMap(string newMap)
    {
        playerInput.SwitchCurrentActionMap(newMap);
    }
}
