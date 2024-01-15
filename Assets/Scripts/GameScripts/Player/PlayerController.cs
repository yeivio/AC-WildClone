using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public GameObject cam; //Player camera

    private float currentSpeed; //currentSpeed
    private float sprintSpeed = 12f; //Sprint player speed
    private float walkSpeed = 7f; //Walk player speed
    private float turnSmoothTime= 0.1f;
    private float turnSmoothVelocity = 0f;

    [SerializeField] private InputActionAsset inputAction;
    private InputAction joystick_input;
    private InputAction sprint_input; // Sprint Button input

    private Vector2 movementInput; // Input readed from the inputmap
    private Vector3 direction; // Vector3 created from the movementInput vector2

    private PlayerAnimationManager playerModelAnimator;    // Player animation manager
    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private ParticleSystem playerParticles;
    private float defaultPlayerParticles = 0.78f;
    private float sprintingPlayerParticles = 1.5f;

    private bool canMove; // Variable for enable/disable player movement


    // Show time while static variables
    private TimerMenuManager timerMenuManager;
    private float notMovedTimer; // Timer that counts the time of the player when it's static

    private float originalValue_Y;


    private void Start()
    {
        this.timerMenuManager = GameObject.FindAnyObjectByType<TimerMenuManager>();
        notMovedTimer = 0;
        playerModelAnimator = this.gameObject.GetComponent<PlayerAnimationManager>();
        joystick_input = this.inputAction.FindActionMap(Utils.FREEMOVE_INPUTMAP).FindAction(Utils.FREEMOVE_MOVE);
        sprint_input = this.inputAction.FindActionMap(Utils.FREEMOVE_INPUTMAP).FindAction(Utils.FREEMOVE_SPRINT);
        canMove = true;
        originalValue_Y = this.gameObject.transform.position.y;
    }

    // Update is called once per frame
    [System.Obsolete]
    private void Update()
    {
        if (!canMove)   // When movement is disable, you can't move
            return;

        movementInput = joystick_input.ReadValue<Vector2>();
        direction = new Vector3(movementInput[0], 0, movementInput[1]);

        /* Check if sprinting button is pressed */
        if (sprint_input.IsPressed())
            this.currentSpeed = sprintSpeed;
        else
            this.currentSpeed = walkSpeed;

        // Movement
        if (direction.magnitude >= 0.1f)
        {
            notMovedTimer = 0; // Reset timer
            if (timerMenuManager.isActive())
                timerMenuManager.hideTimer(); // Hide the time

            if (!walkingSound.isPlaying)
                walkingSound.Play();

            //Particles 
            if(!playerParticles.isPlaying)
                playerParticles.Play();
            if(this.currentSpeed == sprintSpeed) { playerParticles.startSize = sprintingPlayerParticles; } else { playerParticles.startSize = defaultPlayerParticles; }

            //Animation
            playerModelAnimator.getActivePlayerAnimator().SetBool("isMoving", true);


            // This is from this video from brackeys: www.youtube.com/watch?v=4HpC--2iowE
            // I couldn't get the exact fluent movement from the animal crossing game, but 
            // this guy had the most similar one.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * currentSpeed * Time.deltaTime);

        }
        else
        {
            this.playerParticles.Stop();
            walkingSound.Stop();
            playerModelAnimator.getActivePlayerAnimator().SetBool("isMoving", false);

            this.notMovedTimer += Time.deltaTime;
            if (!timerMenuManager.isActive() && notMovedTimer >= 5 && canMove)
                timerMenuManager.showTimer();


        }

        // This is in case the player starts levitating for whatever reason 
        if (this.gameObject.transform.position.y >= originalValue_Y + 1)
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, originalValue_Y, this.gameObject.transform.position.z);
    }


    public void disableMovement()
    {
        this.canMove = false;
    }

    public void enableMovement()
    {
        this.canMove = true;
    }
}
