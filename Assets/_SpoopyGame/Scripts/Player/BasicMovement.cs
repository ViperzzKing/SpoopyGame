using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraControls camControls;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement")]
    private float currentSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float fallingGravity;

    Vector3 moveDirection;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchYScale;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("States")]
    [SerializeField] private State currentPlayerState;
    
    public enum State
    {
        Walk,
        Sprint,
        Crouch,
        Fall
    }


    //---------------------------------------------------------\\


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
        switch (currentPlayerState)
        {
            case State.Walk:
                WalkState();
                break;
            case State.Sprint:
                SprintState();
                break;
            case State.Crouch:
                CrouchState();
                break;
            case State.Fall:
                FallState();
                break;
        }   
    }

    private void SwitchPlayerStates()
    {
        // Sprinting
        if (IsGrounded() && Input.GetKey(KeyCode.R))
        {
            currentPlayerState = State.Sprint;
            currentSpeed = sprintSpeed;
        }

        // Walking
        else if (IsGrounded())
        {
            currentPlayerState = State.Walk;
            currentSpeed = walkSpeed;
        }

        // Falling
        else
        {
            float fallSpeed = crouchSpeed;

            currentPlayerState = State.Fall;
            currentSpeed = fallSpeed;
        }

        if (Input.GetKey(KeyCode.C))
        {
            currentPlayerState = State.Crouch;
            currentSpeed = crouchSpeed;
        }
    }


    //---------------------- STATES --------------------------\\


    private void WalkState()
    {
        MovingPlayer();
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


        // Switch
        if (IsGrounded())
        {
            currentPlayerState = State.Walk;
        }
    }

    
    //------------------- MOVEMENT HANDLER -----------------\\


    private Vector3 MovementInputs() //TODO -- Switch to new input System???? yes or no???
    {                                                                 // ANSWER = 
        Vector2 input = new Vector2();

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(input.x, 0, input.y).normalized;

        //// Rotates player to match camera
        transform.localEulerAngles = new Vector3(0, cam.localEulerAngles.y);

        // Makes the direction from world, to local
        moveDirection = transform.TransformDirection(moveDirection);

        return moveDirection;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void MovingPlayer()
    {
        Vector3 movementInput = MovementInputs();
        movementInput *= currentSpeed;

        // Makes it so we dont build up falling speed
        movementInput.y = Mathf.Clamp(rb.linearVelocity.y - fallingGravity * Time.deltaTime, 0f, float.PositiveInfinity);

        if (OnSlope())
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.localScale = new Vector3(1, crouchYScale, 1);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            camControls.currentEyeOffset = camControls.crouchingEyeOffset;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            // Scale Player
            transform.localScale = new Vector3(1, 1, 1);

            // Position Camera
            camControls.currentEyeOffset = camControls.standingEyeOffset;
        }
    }


    //-------------------- GROUND HANDLER -----------------\\


    private bool IsGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, 0.3f, groundLayer).Length > 0;
    }

    private bool OnSlope()
    {                                                                                 // How far to check
        bool onSlope = Physics.Raycast(transform.position, Vector3.down, out slopeHit, 2 * 0.5f + 0.3f);

        if (onSlope)
        {
            // Check Slopes Steepness
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
}
