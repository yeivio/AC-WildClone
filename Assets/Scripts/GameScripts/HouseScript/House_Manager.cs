using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class House_Manager : MonoBehaviour
{
    protected PlayerController player;
    [SerializeField] private GameObject playerExitPosition;

    private Vector3 exitPlayerPosition;
    private bool isTransitioning;

    protected TitleMenuController titleMenuController;


    private void Start()
    {
        isTransitioning = false;
        if (exitPlayerPosition != null)
            exitPlayerPosition = playerExitPosition.transform.position;
        else
            exitPlayerPosition = Vector3.zero;
        this.titleMenuController = FindAnyObjectByType<TitleMenuController>();
    }

    /// <summary>
    /// When a playerController enters the house, we display an animation of transition into the house with the player on 
    /// walking animation state.
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && !isTransitioning)
        {
            this.player = other.gameObject.GetComponent<PlayerController>();
            this.PlayEnterHouseAnimation(new Vector3(other.transform.position.x,other.transform.position.y,other.transform.position.z + 3f));
        }
    }


    /// <summary>
    /// Plays the animation of entering a house, the endPositions is where the player should be when the animation is finished
    /// During the animation the player input is disabled. 
    /// When the animation is finished it will call the post-Transition function.
    /// </summary>
    /// <param name="endPosition"></param>
    public void PlayEnterHouseAnimation(Vector3 endPosition)
    {
        isTransitioning = true;
        player.disableMovement();
        player.GetComponent<PlayerInteractionsController>().UnequipItem();
        StartCoroutine(EnterHouse(endPosition));
        titleMenuController.gameObject.SetActive(false);
        titleMenuController.gameObject.SetActive(true);
    }

    public void PlayExitHouseAnimation()
    {
        
        isTransitioning = true;
        player.disableMovement();
        StartCoroutine(ExitHouse(new Vector3(this.exitPlayerPosition.x, player.transform.position.y, this.exitPlayerPosition.z)));
        titleMenuController.PlayFadeOut();
    }



    /// <summary>
    /// Playing animation for playing entering the house
    /// </summary>
    /// <param name="endPosition"></param>
    /// <param name="levelNumber"></param>
    /// <returns></returns>
    IEnumerator EnterHouse(Vector3 endPosition)
    {
       
        float duration = 1f;
        float timer = 0;
        Vector3 initialPos = player.transform.position;
        Vector3 finalPos = endPosition;
        while (timer < duration)
        {
            player.transform.position = Vector3.Lerp(initialPos, finalPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        player.transform.position = finalPos;
        player.transform.eulerAngles = new Vector3(180, 180, 180);
        player.enableMovement();
        WhenHouseEnter();
        isTransitioning = false;
    }


    IEnumerator ExitHouse(Vector3 endPosition)
    {
        isTransitioning = true;
        float duration = 1f;
        float timer = 0;
        Vector3 initialPos = player.transform.position;
        Vector3 finalPos = endPosition;
        while (timer < duration)
        {
            player.transform.position = Vector3.Lerp(initialPos, finalPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        player.transform.position = finalPos;
        player.enableMovement();
        isTransitioning = false;

    }

    public abstract void WhenHouseEnter();
}
