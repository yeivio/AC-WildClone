using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerInteractionsController;

public class PlayerAnimationManager : MonoBehaviour
{

    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    public void PlayAnimaton(PlayerState state, string name)
    {
        switch (state)
        {
            case PlayerState.EMPTY_HANDS:
                {
                    FindAnimation_EmptyHands(name);
                }break;
            case PlayerState.SHOVEL:
                {
                    FindAnimation_Shovel(name);
                }
                break;
        }
    }


    private void FindAnimation_EmptyHands(string name)
    {
        switch (name)
        {
            case "Running":
                {
                    playerAnimator.Play("Running_EmptyHands");
                }
                break;
            case "PickingObject":
                {
                    playerAnimator.Play("PickingObject_EmptyHands");
                }
                break;

            case "ShakingTree":
                {
                    playerAnimator.Play("ShakingTree_EmptyHands");
                }
                break;
            case "Idle":
                {
                    playerAnimator.Play("Idle_EmptyHands");
                }
                break;
        }
    }

    private void FindAnimation_Shovel(string name)
    {
        switch (name)
        {
            case "Running":
                {
                    playerAnimator.Play("Running_Shovel");
                }
                break;
            case "PickingObject":
                {
                    playerAnimator.Play("PickingObject_Shovel");
                }
                break;

            case "ShakingTree":
                {
                    playerAnimator.Play("ShakingTree_Shovel");
                }
                break;
            case "Idle":
                {
                    playerAnimator.Play("Idle_Shovel");
                }
                break;
        }
    }

    public void updatePlayerAnimator()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    public Animator getActivePlayerAnimator()
    {
        return this.playerAnimator;
    }
}
