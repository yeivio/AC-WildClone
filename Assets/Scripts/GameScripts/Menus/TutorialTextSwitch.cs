using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialTextSwitch : MonoBehaviour
{

    private PlayerInput playerInput;
    [SerializeField] private GameObject free_move;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject NPC_Talk;
    [SerializeField] private GameObject NPC_Sell;
    [SerializeField] private GameObject NPC_Buy;


    [SerializeField] private TalkableObject NPC_Seller_OBJ; // Npc player object

    void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        activarUI(free_move);
    }

    // Update is called once per frame
    void Update()
    {
        if(NPC_Seller_OBJ.isActive && NPC_Seller_OBJ.tiendaAbierta)
        {
            if(NPC_Seller_OBJ.isBuyShop)
                activarUI(NPC_Buy);
            else
                activarUI(NPC_Sell);
            return;
        }

        if (playerInput.currentActionMap == playerInput.actions.FindActionMap(Utils.FREEMOVE_INPUTMAP) && !this.free_move.activeSelf)
            activarUI(free_move);
        if (playerInput.currentActionMap == playerInput.actions.FindActionMap(Utils.UI_INPUTMAP) && !this.UI.activeSelf)
            activarUI(UI);
        if(playerInput.currentActionMap == playerInput.actions.FindActionMap(Utils.NPC_TALK_INPUTMAP) && !this.NPC_Talk.activeSelf)
            activarUI(NPC_Talk);
    }

    private void activarUI(GameObject obj)
    {
        free_move.SetActive(false);
        UI.SetActive(false);
        NPC_Talk.SetActive(false);
        NPC_Sell.SetActive(false);
        NPC_Buy.SetActive(false);
        obj.SetActive(true);
    }



}
