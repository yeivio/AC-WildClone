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
    void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        activarUI(free_move);
    }

    // Update is called once per frame
    void Update()
    {
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
        obj.SetActive(true);
    }
}
