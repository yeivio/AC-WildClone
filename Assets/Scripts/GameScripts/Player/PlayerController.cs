using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private MenuManager menuManager;
    
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
            menuManager.OpenPlayerInventory();
            playerInput.SwitchCurrentActionMap(Utils.UI_INPUTMAP);
        }
    }

    public void CloseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(menuManager.CloseMenu())
                playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
        }
    }

}
