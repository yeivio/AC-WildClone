using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;

public class PlayerInteractionsController : MonoBehaviour
{
    [SerializeField] private PlayerInventory_ScriptableObject inventoryData; //Player inventory data
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private AudioSource pickupSound;
    public GameObject cam;  //Player camera

    private GameObject interactingObject; //Object which they player is interacting

    private MovableObject movingObject; // Object the player is moving
    private float moveObjectCooldown = 0.5f; //Cooldown of moving an object
    private float lastMovedTimestamp; // Timestamp of the last moment a player moved an object

    private PlayerAnimationManager playerAnimationManager;
    private PlayerState currentPlayerState;


    // Equipable items
    [SerializeField] private GameObject[] handBone;   //Bone where the equipable items will be placed
    private InventoryItem_ScriptableObject EquipableItem;
    

    public enum PlayerState
    {
        EMPTY_HANDS,
        SHOVEL
    }

    private void Start()
    {
        playerAnimationManager = this.GetComponent<PlayerAnimationManager>();
        currentPlayerState = PlayerState.EMPTY_HANDS;
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
            if (inventoryData.AddItem(pickObj.inventorySprite)) {
                pickObj.takeObject();
                pickupSound.Play();
            }
        }

        if (context.interaction is PressInteraction && // Talking Object
            interactingObject.TryGetComponent<TalkableObject>(out TalkableObject talkObj))
        {
            this.playerInputController.SwitchInputMap(Utils.NPC_TALK_INPUTMAP);
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
        if(!collision.gameObject.CompareTag("LimitWall"))
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
        this.GetComponentInChildren<Animator>().SetInteger("PlayerState", ((int)this.currentPlayerState));  // Change animations
    }

    public void EquipItem(InventoryItem_ScriptableObject item)
    {
        if(this.EquipableItem != null)
        {
            for (int i = 0; i < handBone.Length; i++)
            {
                if (handBone[i].gameObject.activeSelf)
                    handBone[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            //Destroy(GameObject.FindGameObjectsWithTag("EquipableItem")[0]); //Destroy equipped object
        }
        this.EquipableItem = item;

        for (int i = 0; i < handBone.Length; i++){
            if (handBone[i].gameObject.activeSelf)
                handBone[i].gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        //Instantiate(item.equipableModel, this.handBone.transform); // Instantiate object on the 

        this.setPlayerState(PlayerState.SHOVEL);
    }

    /// <summary>
    /// This method unequips an item when the button of the controller is pressed
    /// </summary>
    /// <param name="context"></param>
    public void UnequipItem(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        this.UnequipItem();
    }

    /// <summary>
    /// This method unequips an item whenever it's called
    /// </summary>
    public void UnequipItem()
    {
        if (this.currentPlayerState == PlayerState.EMPTY_HANDS)
            return;
        this.setPlayerState(PlayerState.EMPTY_HANDS);
        if (this.EquipableItem != null)
        {
            for (int i = 0; i < handBone.Length; i++)
            {
                if (handBone[i].gameObject.activeSelf)
                    handBone[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            //Destroy(GameObject.FindGameObjectsWithTag("EquipableItem")[0]); //Destroy equipped object
        }
        this.EquipableItem = null; // Resets
    }

    public InventoryItem_ScriptableObject getEquippedItem()
    {
        if (this.EquipableItem != null)
            return this.EquipableItem;
        return null;
    }

    public void PlayerDig(InputAction.CallbackContext context)
    {
        if (!context.performed || this.currentPlayerState != PlayerState.SHOVEL)
            return;
        if(BuildingSystem.current.Dig(BuildingSystem.current.NextPositionInGrid(this.gameObject),
            BuildingSystem.current.LookingDirection(this.gameObject)))
        {
            this.playerAnimationManager.PlayAnimaton(this.getPlayerState(), "Dig");
        }
    }
}
