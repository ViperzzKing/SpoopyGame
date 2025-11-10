using JetBrains.Annotations;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CapsuleCollider capsuleCollider;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float fallingGravity;

    [Header("States")]
    [SerializeField] private State currentPlayerState;
    
    public enum State
    {
        Walk,
        Sprint,
        Crouch,
        Fall,
        Inspect
    }


    //---------------------------------------------------------\\


    private void Update()
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
                break;
            case State.Inspect:
                break;
        }
    }


    //---------------------- STATES --------------------------\\


    private void WalkState()
    {
        Vector3 movementInput = MovementInputs();
        movementInput *= walkSpeed;

        // Makes it so we dont build up falling speed
        movementInput.y = Mathf.Clamp(rb.linearVelocity.y - fallingGravity * Time.deltaTime, 0f, float.PositiveInfinity);
        

        rb.linearVelocity = movementInput;
    }
    private void SprintState()
    {

    }
    private void CrouchState()
    {

    }
    private void Fall()
    {

    }
    private void InspectState()
    {
        //TODO -- Make Inspecting Mechanic
    }

    
    //------------------- MOVEMENT HANDLER -----------------\\


    private Vector3 MovementInputs() //TODO -- Switch to new input System???? yes or no???
    {                                                                 // ANSWER = 
        Vector2 input = new Vector2();

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

        //// Rotates player to match camera
        transform.localEulerAngles = new Vector3(0, cam.localEulerAngles.y);

        // Makes the direction from world, to local
        moveDirection = transform.TransformDirection(moveDirection);

        return moveDirection;
    }


    //-------------------- GROUND HANDLER -----------------\\


    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, capsuleCollider.height / 2f + 0.1f, groundLayer);
    }
}
