using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInteractionsController : MonoBehaviour
{
    [SerializeField] private PlayerInventory_ScriptableObject inventoryData; //Player inventory data
    private PlayerInput playerInput; //Player input
    public GameObject cam;  //Player camera

    private Coroutine refCoroutines; // Different coroutines references
    public GameObject interactingObject; //Object which they player is interacting

    private MovableObject movingObject; // Object the player is moving
    private float moveObjectCooldown = 0.5f; //Cooldown of moving an object
    private float lastMovedTimestamp; // Timestamp of the last moment a player moved an object
    private void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed || !interactingObject)
            return;

        //this.refCoroutines = StartCoroutine(RotateFixedToInteraction(interactingObject)); //Rotate player

        if (context.interaction is PressInteraction && // Pickup Object
            interactingObject.TryGetComponent<PickableObject>(out PickableObject pickObj))
        {
            Vector3 copia = pickObj.transform.position; //In case we need to respawn the object
            if (!inventoryData.AddItem(pickObj.takeObject()))
            {
                Instantiate(pickObj, copia, Quaternion.identity);
            }
        }

        if (context.interaction is PressInteraction && // Talking Object
            interactingObject.TryGetComponent<TalkableObject>(out TalkableObject talkObj))
        {
            this.playerInput.SwitchCurrentActionMap(Utils.UI_INPUTMAP);
            talkObj.talk();

        }

        if (context.interaction is HoldInteraction && // Movable Object
            interactingObject.TryGetComponent<MovableObject>(out MovableObject movObj))
        {
            movingObject = movObj;
            playerInput.SwitchCurrentActionMap(Utils.MOVING_OBJECTS_INPUTMAP);
        }

        if (interactingObject.TryGetComponent<FishingObject>(out FishingObject fishObj))  // Fishing point
        {
            playerInput.SwitchCurrentActionMap(Utils.FISHING_INPUTMAP);
            fishObj.interaction();
        }
    }

    public void MoveObject(InputAction.CallbackContext context)
    {
        if (Time.time - lastMovedTimestamp < moveObjectCooldown)
            return;
        Vector2 input = context.ReadValue<Vector2>();

        lastMovedTimestamp = Time.time;
        movingObject.GetComponent<MovableObject>().moveObject(input);
    }

    public void ExitMoveObject(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
    }

    IEnumerator RotateFixedToInteraction(GameObject interactingObject)
    {
        float timeElapsed = 0;
        float duration = 0.1f;
        float start = this.gameObject.transform.eulerAngles.y;
        float end = Mathf.Round(start / 90) * 90;
        float lerpedValue;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            lerpedValue = Mathf.Lerp(start, end, t);
            this.gameObject.transform.eulerAngles = new Vector3(this.gameObject.transform.eulerAngles.x, lerpedValue, this.gameObject.transform.eulerAngles.z);
            timeElapsed += Time.deltaTime;
            yield return null; // Stops until next frame
        }
        lerpedValue = end;
        this.gameObject.transform.eulerAngles = new Vector3(this.gameObject.transform.eulerAngles.x, lerpedValue, this.gameObject.transform.eulerAngles.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        interactingObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        interactingObject = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        interactingObject = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        interactingObject = null;
    }

}
