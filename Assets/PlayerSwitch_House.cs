using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwitch_House : House_Manager
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    public override void WhenHouseEnter()
    {
        if(player1.activeSelf) {
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(true);
        }
        else { 
            player2.gameObject.SetActive(false);
            player1.gameObject.SetActive(true);
            
        }

        player.GetComponent<PlayerAnimationManager>().updatePlayerAnimator(); // Update animator
        player.transform.Rotate(new Vector3(0, 180, 0)); // Rotate player
        this.PlayExitHouseAnimation();  // Play exit animation
    }
}
