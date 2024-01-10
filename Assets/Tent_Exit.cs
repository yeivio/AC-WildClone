using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tent_Exit : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerHouseManager houseManager;

    private void OnEnable()
    {
        houseManager = FindAnyObjectByType<PlayerHouseManager>();
        player = FindAnyObjectByType<PlayerController>();
        this.PlayEnterHouseAnimation(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 5f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            this.player = other.gameObject.GetComponent<PlayerController>();
            player.disableMovement();
            StartCoroutine(ExitHouse(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z - 3f)));
            
        }
    }


    /// <summary>
    /// Plays the animation of entering a house, the endPositions is where the player should be when the animation is finished
    /// During the animation the player input is disabled. 
    /// When the animation is finished, if the function has an Scene number as an argument, it will load the scene.
    /// Also the hitbox collider for exit the house is activated when finished
    /// </summary>
    /// <param name="endPosition"></param>
    public void PlayEnterHouseAnimation(Vector3 endPosition)
    {
        player.disableMovement();
        StartCoroutine(EnterHouse(endPosition));
    }

    IEnumerator ExitHouse(Vector3 endPosition)
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
        player.transform.position = endPosition;
        player.enableMovement();
        //Activar hitbox para salir
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        houseManager.ExitHouse();
        
        this.transform.parent.gameObject.SetActive(false);
        yield return null;
    }
   

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
        player.enableMovement();
        //Activar hitbox para salir
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        
    }
}
