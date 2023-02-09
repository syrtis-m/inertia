using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    // Movement variable section
    // Note: [Header("Movement")] creates a bolded header which helps organize variables
    [Header("Movement")]
    // Sets max speed of player movement
    private float maxSpeed;
    public float walkSpeed;
    public float wallRunSpeed;

    //Sets how much drag
    public float groundDrag;

    // Stores player orientation
    public Transform orientation;

    // Setup ground checking
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    // Stores keyboard inputs
    float horizontalInput;
    float verticalInput;

    // Jump variables
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultipier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    // Stores calculated move direction
    Vector3 moveDirection;

    // Stores the rigid body of our player
    Rigidbody rb;

    // State variable will contain our movement state based on the enum
    public MovementState state;

    // Enum with all our states
    public enum MovementState
    {
        walking,
        wallRunning,
        jumping
    }

    // State booleans
    public bool wallRunning;

    // StateHandler function works with MovementState enum
    private void StateHandler()
    {
        if(wallRunning)
        {
            state = MovementState.wallRunning;
            maxSpeed = wallRunSpeed;
        }

        else if (grounded)
        {
            state = MovementState.walking;
            maxSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.jumping;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize ready to jump
        readyToJump = true;

        // Our camera will rotate for us, no need for the rigid body to rotate
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Perform ground check using raycasting
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // We will have no drag in the air so only apply drag when grounded
        if(grounded)
        {
            rb.drag = groundDrag;
        }  
        else
        {
            rb.drag = 0;
        }

        // Call functions that need update
        GetInput();
        SpeedLimit();
        StateHandler();
    }

    // FixedUpdate() updates based on frame rate
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Get the inputs from the keyboard
    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Check if jump is pressed and authorized
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            // Perform jump
            readyToJump = false;
            Jump();

            // Allow jump key to be held down
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    // Main player movement function
    private void MovePlayer()
    {
        // Walk in the direction we are looking
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Add the force to make our player actually move, when in the air use air multiplier
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * maxSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * maxSpeed * 10f * airMultipier, ForceMode.Force);
        }
    }

    // Limit speed to the player movement speed to maxSpeed
    private void SpeedLimit()
    {
        Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit speed if magnitude of velocity is greater than the max speed
        if(velocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    // Jumping capability implementation
    private void Jump()
    {
        // Reset y velocity to ensure all jumps are the same height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Add force to make player jump, use ForceMode.Impulse because force only applied once
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // Reset readyToJump to true
    private void ResetJump()
    {
        readyToJump = true;
    }
}
