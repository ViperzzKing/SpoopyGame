using System.Threading;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public static BasicMovement Instance;
    
    [Header("References")]
    [SerializeField] private CameraControls camControls;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private PlayerSounds playerSounds;
    private Noise playerNoise;

    [Header("Movement Settings")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    Vector3 moveDirection;


    [Header("Sounds")]
    [SerializeField] private float walkVolume;
    [SerializeField] private float crouchVolume;
    [SerializeField] private float sprintVolume;
    [SerializeField] private float currentVolume;
    
    
    [Header("Crouching")]
    [SerializeField] private float crouchYScale;

    [Header("Physics")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float fallingGravity;
    [SerializeField] private LayerMask groundLayer;
    private RaycastHit slopeHit;

    [Header("Keybinds")] 
    [SerializeField] private KeyCode crouchKeybind = KeyCode.C;
    [SerializeField] private KeyCode sprintKeybind = KeyCode.LeftShift;

    [Header("States")]
    public PlayerState CurrentState { get; private set; }
    public PlayerState PreviousState { get; private set; }


    public enum PlayerState
    {
        Walk,
        Sprint,
        Crouch,
        Fall
    }


    //---------------------------------------------------------\\

    private void Awake()
    {
        Instance = this;
        playerNoise = GetComponent<Noise>();
    }

    private void Update()
    {
        SwitchPlayerStates();
        InputManager();
    }

    private void FixedUpdate()
    {
        ManagePlayerStates();
    }

    private void ManagePlayerStates()
    {
        switch (CurrentState)
        {
            case PlayerState.Walk:
                WalkState();
                break;
            case PlayerState.Sprint:
                SprintState();
                break;
            case PlayerState.Crouch:
                CrouchState();
                break;
            case PlayerState.Fall:
                FallState();
                break;
        }   
    }

    private void SwitchPlayerStates()
    {
        bool sprintKeybindHeld = Input.GetKey(sprintKeybind);
        bool crouchKeybindHeld = Input.GetKey(crouchKeybind);
        
        // Sprinting
        if (IsGrounded() && sprintKeybindHeld && !crouchKeybindHeld)
        {
            PreviousState = CurrentState;
            CurrentState = PlayerState.Sprint;
            
            currentSpeed = sprintSpeed;
            currentVolume = sprintVolume;
        }

        // Walking
        else if (IsGrounded())
        {
            PreviousState = CurrentState;
            CurrentState = PlayerState.Walk;
            
            currentSpeed = walkSpeed;
            currentVolume = walkVolume;
        }
        else if (crouchKeybindHeld)
        {
            PreviousState = CurrentState;
            CurrentState = PlayerState.Crouch;
            
            currentSpeed = crouchSpeed;
            currentVolume = crouchVolume;
        }
        // Falling
        else
        {
            float fallSpeed = crouchSpeed;

            PreviousState = CurrentState;
            CurrentState = PlayerState.Fall;
            currentSpeed = fallSpeed;
            currentVolume = crouchSpeed;
        }

        
        if (PreviousState != CurrentState && CurrentState != PlayerState.Fall)
        {
            playerSounds.ChangeAudioSource();
        }
        
        if (rb.linearVelocity.magnitude < 0.05f || CurrentState == PlayerState.Fall)
        {
            playerSounds.StopAudio(playerSounds.GetCurrentAudioSource());
        }
        else
        {
            playerSounds.PlayAudio(playerSounds.GetCurrentAudioSource());
        }
    }


    //---------------------- STATES --------------------------\\


    private void WalkState()
    {
        
        MovingPlayer(); // Run MovingPlayer
    }
    private void SprintState()
    {
        MovingPlayer();
    }
    private void CrouchState()
    {
        MovingPlayer();
    }
    private void FallState()
    {
        Vector3 movementInput = MovementInputs();
        movementInput *= walkSpeed;

        // Apply Gravity
        movementInput.y = rb.linearVelocity.y - fallingGravity * Time.deltaTime;

        rb.linearVelocity = movementInput;


        // Switch to walk
        if (IsGrounded())
        {
            CurrentState = PlayerState.Walk;
        }
    }

    
    //------------------- MOVEMENT HANDLER -----------------\\


    private Vector3 MovementInputs() //TODO -- Switch to new input System???? yes or no???
    {                                                                 // ANSWER = 
        Vector2 input = new Vector2();

        //WASD
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Normalize movement and get move direction
        moveDirection = new Vector3(input.x, 0, input.y).normalized;

        //// Rotates player to match camera
        transform.localEulerAngles = new Vector3(0, cam.localEulerAngles.y);

        // Makes the direction from world, to local
        moveDirection = transform.TransformDirection(moveDirection);

        return moveDirection;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        // changes normal direction so it matches the slop
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void MovingPlayer()
    {
        Vector3 movementInput = MovementInputs();
        movementInput *= currentSpeed;

        if (CurrentState == PlayerState.Walk)
            movementInput.y = 0;

        if (IfOnSlope())
        {
            // Move along the slope direction instead of directly forward
            Vector3 slopeDirection = GetSlopeMoveDirection() * currentSpeed;

            rb.linearVelocity = slopeDirection;
        }
        else
        {
            rb.linearVelocity = movementInput;
        }


    }
    
    //------------------- INPUT HANDLER --------------------\\


    private void InputManager()
    {
        // Start Crouching
        if (Input.GetKeyDown(crouchKeybind))
        {
            transform.localScale = new Vector3(1, crouchYScale, 1); // change our scale
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // go straight down

            camControls.SetEyeOffSet(camControls.CrouchingEyeOffset); // change eye offset
        }

        if (Input.GetKeyUp(crouchKeybind)) // uncrouch player
        {
            // Scale Player
            transform.localScale = new Vector3(1, 1, 1);

            // Position Camera
            camControls.SetEyeOffSet(camControls.StandingEyeOffset);
        }
    }


    //-------------------- GROUND HANDLER -----------------\\


    private bool IsGrounded()
    {
        //Overlap sphere check
        return Physics.OverlapSphere(groundCheck.position, 0.5f, groundLayer).Length > 0;
    }

    private bool IfOnSlope()
    {                                                                                                // How far to check
        bool onSlope = Physics.Raycast(transform.position, Vector3.down, out slopeHit, 2 * 0.5f + 0.3f);

        if (onSlope)
        {
            // Check Slopes Steepness
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false; // not on slope
    }
}
