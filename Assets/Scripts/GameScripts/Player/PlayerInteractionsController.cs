using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInteractionsController : MonoBehaviour
{
    [SerializeField] private PlayerInventory_ScriptableObject inventoryData; //Player inventory data
    [SerializeField] private PlayerInputController playerInputController;
    public GameObject cam;  //Player camera

    private Coroutine refCoroutines; // Different coroutines references
    public GameObject interactingObject; //Object which they player is interacting

    private MovableObject movingObject; // Object the player is moving
    private float moveObjectCooldown = 0.5f; //Cooldown of moving an object
    private float lastMovedTimestamp; // Timestamp of the last moment a player moved an object

    private PlayerAnimationManager playerAnimationManager;
    private PlayerState currentPlayerState;

    public bool hasShovel;
    private GameObject shovel_item;

    public enum PlayerState
    {
        EMPTY_HANDS,
        SHOVEL
    }

    private void Start()
    {
        playerAnimationManager = this.GetComponent<PlayerAnimationManager>();
        currentPlayerState = PlayerState.EMPTY_HANDS;
        shovel_item = GameObject.FindGameObjectWithTag("Shovel");   // Find Shovel
        shovel_item.SetActive(false);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed || !interactingObject)
            return;

        //this.refCoroutines = StartCoroutine(RotateFixedToInteraction(interactingObject)); //Rotate player

        if (context.interaction is PressInteraction && // Pickup Object
            interactingObject.TryGetComponent<PickableObject>(out PickableObject pickObj))
        {
            this.playerAnimationManager.PlayAnimaton(this.getPlayerState(), "PickingObject");
            Vector3 copia = pickObj.transform.position; //In case we need to respawn the object
            if (!inventoryData.AddItem(pickObj.inventorySprite))
            {
                Instantiate(pickObj, copia, Quaternion.identity);
            }
            else{
                pickObj.takeObject();
            }
        }

        if (context.interaction is PressInteraction && // Talking Object
            interactingObject.TryGetComponent<TalkableObject>(out TalkableObject talkObj))
        {
            this.playerInputController.SwitchInputMap(Utils.UI_INPUTMAP);
            talkObj.talk(this);

        }

        if (context.interaction is HoldInteraction && // Movable Object
            interactingObject.TryGetComponent<MovableObject>(out MovableObject movObj))
        {
            movingObject = movObj;
            this.playerInputController.SwitchInputMap(Utils.MOVINGOBJECTS_INPUTMAP);
        }

        if (interactingObject.TryGetComponent<FishingObject>(out FishingObject fishObj))  // Fishing point
        {
            this.playerInputController.SwitchInputMap(Utils.FISHING_INPUTMAP);
            fishObj.interaction();
        }

        if (interactingObject.TryGetComponent<TreeManager>(out TreeManager treeObj))  // Tree object
        {
            this.PlayerShakesTree(treeObj);
            treeObj.shakeTree();
        }
    }


    public void PlayerShakesTree(TreeManager treeObj)
    {
        switch (treeObj.getTreeState())
        {
            case TreeManager.TreeState.GROWING:
                {
                    
                }
                break;
            case TreeManager.TreeState.FULL_GROWN:
                {
                    this.playerAnimationManager.PlayAnimaton(this.getPlayerState(), "ShakingTree");
                }
                break;
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
        this.playerInputController.SwitchInputMap(Utils.FREEMOVE_INPUTMAP);
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
    public PlayerState getPlayerState()
    {
        return this.currentPlayerState;
    }


    public void setPlayerState(PlayerState state)
    {
        this.currentPlayerState = state;
        this.GetComponentInChildren< Animator>().SetInteger("PlayerState", ((int)this.currentPlayerState));
        if (state == PlayerState.SHOVEL)
            shovel_item.SetActive(true);
        else
            shovel_item.SetActive(false);
    }

    public void EquipShovel()
    {
        shovel_item.SetActive(true);
        this.setPlayerState(PlayerState.SHOVEL);
    }

    public void SaveShovel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (this.currentPlayerState == PlayerState.EMPTY_HANDS)
            return;
        shovel_item.SetActive(false);
        this.setPlayerState(PlayerState.EMPTY_HANDS);
    }
}
