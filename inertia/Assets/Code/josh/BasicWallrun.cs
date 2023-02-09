using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWallrun : MonoBehaviour
{
    // Variables used for wallrunning
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    // Keyboard input variables
    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;
    private bool upwardsRunning;
    private bool downwardsRunning;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;

    // Variables used to detect walls on right and left
    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;
    
    // Include reference to the player orientation, basic movement script, and player rigid body
    [Header("References")]
    public Transform orientation;
    private BasicMovement basicMovement;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        basicMovement = GetComponent<BasicMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    // If we are in the walling state use the wallrunning functionality
    private void FixedUpdate()
    {
        if (basicMovement.wallRunning)
        {
            WallRunningMovement();
        }
    }

    // Check for wall on the right and left and store the object that we hit as a Raycast
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    // Returns true if the Raycast hits nothing meaning we are above the ground
    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    // 2 wallrunning states defined 1: Wallrunning, 2: None
    private void StateMachine()
    {
        // Get inputs from the keyboard
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            if (!basicMovement.wallRunning)
            {
                StartWallRun();
            }
        }

        // State 2: None
        else
        {
            if (basicMovement.wallRunning)
            {
                StopWallRun();
            }
        }
    }

    // Start wall run by telling basic movement to change states
    private void StartWallRun()
    {
        basicMovement.wallRunning = true;
    }

    // Stop wall run by telling basic movement to change states
    private void StopWallRun()
    {
        basicMovement.wallRunning = false;
    }

    // Main functionality of a wallrun
    private void WallRunningMovement()
    {
        // Turn gravity off for now
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Gives us the normal vectore for the wall we hit
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        // This gives us the forward direction for the wallrun
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        // Determine which way the player is facing and apply the proper wall run direction
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        // Implement the forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // Implement upwards and downwards running forces
        if (upwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        }
        if (downwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
        }

        // Push player to allow wall running on curved walls, only apply force if player is not trying to get away from the wall
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
    }
}
