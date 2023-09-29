using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float currentSpeed = 7f; //currentSpeed
    public float sprintSpeed = 12f; //Sprint player speed
    public float walkSpeed = 7f; //Walk player speed
    public float turnSmoothTime= 0.1f;
    public float turnSmoothVelocity= 0f;

    private Coroutine refCoroutines; // Different coroutines references

    private GameObject interactingObject; //Object which they player is interacting

    [SerializeField] private PlayerInventoryManager playerInventory;
    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    private void Update()
    {

        /*      OLD SYSTEM      */

        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        Vector2 movementInput = playerInput.actions[Utils.MOVE_INPUT].ReadValue<Vector2>();
        Vector3 direction = new Vector3(movementInput[0], 0, movementInput[1]);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * currentSpeed * Time.deltaTime);
        }

    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            this.currentSpeed = sprintSpeed;
        else
            this.currentSpeed = walkSpeed;
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInventory.gameObject.SetActive(true);
            playerInput.SwitchCurrentActionMap(Utils.UI_INPUTMAP);
        }
    }

    public void TakeObject(InputAction.CallbackContext context)
    {
        if (!context.performed || !interactingObject)
            return;
        this.refCoroutines = StartCoroutine(RotateToInteraction(interactingObject)); //Girar
        if (interactingObject.TryGetComponent<PickableObject>(out PickableObject obj))
        {            
            obj.takeObject();
            playerInventory.AddItem(obj.getIcon());
            
        }
    }
    public void MoveObject(InputAction.CallbackContext context)
    {
        if (!context.performed || !interactingObject)
            return;

        this.refCoroutines = StartCoroutine(RotateToInteraction(interactingObject)); //Girar
        if (interactingObject.TryGetComponent<MovableObject>(out MovableObject obj))
        {
            obj.moveObject();
        }

    }

    IEnumerator RotateToInteraction(GameObject interactingObject)
    {
        float timeElapsed = 0;
        float duration = 0.1f;
        float start = this.gameObject.transform.eulerAngles.y;
        float end = Mathf.Round(start / 90) * 90;
        float lerpedValue;
        while(timeElapsed < duration)
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
