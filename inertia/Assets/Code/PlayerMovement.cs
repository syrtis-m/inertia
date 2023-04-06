//Adopted from https://www.youtube.com/watch?v=55WCcJi79QM&t=0s

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, Controls.IPlayerActions
{
    //we inherit from Controls.IPlayerActions so we can use SetCallbacks()
    
    //singleton pattern
    public static PlayerMovement instance;
    
    //UNITY input actions (controls.cs)
    private Controls controls; 
    private Vector2 MousePos;
    private Vector2 MoveComposite;
    private Action OnJumpPerformed;
    public CharacterController controller;
    
    
    public Transform groundCheck;

    public LayerMask groundMask;
    public LayerMask wallMask;

    Vector3 move;
    Vector3 input;
    Vector3 Yvelocity;
    Vector3 forwardDirection;

    private float speed;

    //movement configs
    public float runSpeed;
    public float airSpeedMultiplier; //slows you down when you're in midair
    public float wallJumpSpeedMult; //impacts how flat the parabola of jumping off is
    public float walljumpNormalMagnitude = 1f; //how much sideways you go when you jump off a wall (magnitude modifier for normal vector)

    private float gravity;
    public float normalGravity;
    public float wallRunGravity;
    public float maxGravity;//this configs what the max yvelocity.y there can be when standing on a platform. (this impacts how it feels to walk off a platform without jumping. this is less than normalGravity to give a similar feeling to coyote time mechanics
    public float jumpHeight;

    public float wallRunSpeedIncrease;
    public float wallRunSpeedDecrease;
    
    int jumpCharges;

    public bool isSprinting;
    bool isWallRunning;
    bool isGrounded; //are we currently on the ground.

    //float startHeight;
    //Vector3 standingCenter = new Vector3(0, 0, 0);

    bool onLeftWall;
    bool onRightWall;
    bool hasWallRun = false;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    Vector3 wallNormal;
    Vector3 lastWall;

    bool isWallJumping;
    float wallJumpTimer;
    public float maxWallJumpTimer;

    public Camera playerCamera;
    float normalFov;
    public float specialFov;
    public float cameraChangeTime;
    public float wallRunTilt;
    public float tilt;

    public Vector3 externalMovement; //used by moving platforms

    /////// unity input stuff ///////
    private void OnEnable()
    {
        if (controls != null)
            return;

        controls = new Controls();
        controls.Player.Enable();
        controls.Player.SetCallbacks(this); //this lets OnLook OnMove OnJump work
    }
    public void OnDisable()
    {
        controls.Player.Disable();
    }
    
    //On[ACTION] for each Action in the InputAction Asset
    public void OnLook(InputAction.CallbackContext context)
    { //currently isn't used for camera controls.
        MousePos = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (jumpCharges > 0)
        {
            Jump();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {//https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Interactions.html
        //this function doesn't actually do anything - dash is implemented in PlayerDash.cs
        var held = context.control.IsPressed();
        if (held && isGrounded)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void ResetPlayer()
    {
        jumpCharges = 2;
        normalFov = playerCamera.fieldOfView;
        
        //movement stuff
        speed = 0;
        move = Vector3.zero;
        input = Vector3.zero;
        Yvelocity = Vector3.zero;
        forwardDirection = Vector3.zero;

    }

    void Start()
    {
        Application.targetFrameRate = 60; //target 60fps. BUG: game plays differently (parabolas and accel curves) are different at different frame rates
        controller = GetComponent<CharacterController>();
        //startHeight = transform.localScale.y; don't think this is used, so commenting out
        jumpCharges = 2;
        normalFov = playerCamera.fieldOfView;
    }

    void IncreaseSpeed(float speedIncrease)
    {
        speed += speedIncrease * Time.deltaTime;
    }

    void DecreaseSpeed(float speedDecrease)
    {
        speed -= speedDecrease * Time.deltaTime;
    }

    void Update()
    {
        
        input = new Vector3(MoveComposite.x , 0f, MoveComposite.y).normalized;
        
        if (!isWallRunning)
        {//does this make you not change directions on the wall?
            input = transform.TransformDirection(input);
            input = Vector3.ClampMagnitude(input, 1f);
        }
        
        CheckWallRun();
        CheckGround();
        if (isGrounded)
        {
            GroundedMovement();
        }
        else if (!isGrounded && !isWallRunning)
        {
            AirMovement();
        }
        else if (isWallRunning)
        {
            WallRunMovement();
            DecreaseSpeed(wallRunSpeedDecrease);
        }
        
        
        CameraEffects();
        ApplyGravity();
    }

    void FixedUpdate()
    {
        controller.Move(move * Time.deltaTime);
        controller.Move(Yvelocity * Time.deltaTime);
        if (isGrounded)
        {
            controller.Move(externalMovement); 
        }
        
    }

    void CameraEffects()
    {
        float fov = isWallRunning ? specialFov : normalFov;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, cameraChangeTime * Time.deltaTime);

        if (isWallRunning)
        {
            if (onRightWall)
            {
                tilt = Mathf.Lerp(tilt, wallRunTilt, cameraChangeTime * Time.deltaTime);
            }
            else if (onLeftWall)
            {
                tilt = Mathf.Lerp(tilt, -wallRunTilt, cameraChangeTime * Time.deltaTime);
            }
        }
        else
        {
            tilt = Mathf.Lerp(tilt, 0f, cameraChangeTime * Time.deltaTime);
        }
    }


    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.25f, groundMask);
        if (isGrounded)
        {
            jumpCharges = 1;
            hasWallRun = false;
        }
    }

    void CheckWallRun()
    {
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 0.9f, wallMask);
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.9f, wallMask);

        if ((onRightWall || onLeftWall) && !isWallRunning && !isGrounded)
        {
            TestWallRun();
        }
        else if ((!onRightWall && !onLeftWall) && isWallRunning)
        {
            ExitWallRun();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.right);
        Gizmos.DrawRay(transform.position, -transform.right);
        Gizmos.DrawSphere(groundCheck.position, 0.25f);
    }

    void GroundedMovement()
    {
        speed = runSpeed;

        if (input.x != 0)
        {
            move.x += input.x * speed;
        }
        else
        {
            move.x = 0;
        }
        if (input.z != 0)
        {
            move.z += input.z * speed;
        }
        else
        {
            move.z = 0;
        }
         
        move = Vector3.ClampMagnitude(move, speed);
    }

    void AirMovement()
    {
        move.x += input.x * airSpeedMultiplier; 
        move.z += input.z * airSpeedMultiplier;
        if (isWallJumping)
        {
            move += (walljumpNormalMagnitude * forwardDirection) * (airSpeedMultiplier * wallJumpSpeedMult);
            wallJumpTimer -= 1f * Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                isWallJumping = false;
            }
        }

        move = Vector3.ClampMagnitude(move, speed);
    }

    void WallRunMovement()
    {
        float cameraAng = 20f;
        if (input.z > (forwardDirection.z - cameraAng) && input.z < (forwardDirection.z + cameraAng))
        {
            move += forwardDirection;
        }
        else if (input.z < (forwardDirection.z - cameraAng) || input.z > (forwardDirection.z + cameraAng))
        {
            move.x = 0;
            move.z = 0;
            ExitWallRun();
        }
        move.x += input.x * airSpeedMultiplier;

        move = Vector3.ClampMagnitude(move, speed);
    }

    void TestWallRun()
    {
        wallNormal = onRightWall ? rightWallHit.normal : leftWallHit.normal;
        if (hasWallRun)
        {
            float wallAngle = Vector3.Angle(wallNormal, lastWall);
            if (wallAngle > 15)
            {
                WallRun();
            }
        }
        else
        {
            hasWallRun = true;
            WallRun();
        }
    }

    void WallRun()
    {
        isWallRunning = true;
        jumpCharges = 1;
        IncreaseSpeed(wallRunSpeedIncrease);
        Yvelocity = new Vector3(0f, 0f, 0f);

        forwardDirection = Vector3.Cross(wallNormal, Vector3.up);

        if (Vector3.Dot(forwardDirection, transform.forward) < 0)
        {
            forwardDirection = -forwardDirection;
        }
    }

    void ExitWallRun()
    {
        isWallRunning = false;
        lastWall = wallNormal;
        forwardDirection = wallNormal;
        IncreaseSpeed(wallRunSpeedIncrease);
    }

    void Jump()
    {
        if (!isGrounded && !isWallRunning)
        {
            jumpCharges -= 1;//jump charges only subtract when you aren't grounded or wallrunning
            Yvelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);//jump
        }
        else if (isWallRunning)
        { 
            ExitWallRun();
            isWallJumping = true;
            wallJumpTimer = maxWallJumpTimer;
            Yvelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);//jump
        }
        else
        {
            Yvelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);//jump
        }
    }

    void ApplyGravity()
    {
        gravity = isWallRunning ? wallRunGravity : normalGravity;
        //check if we are at constant transform.position.y; if so, clamp yvelocity. otherwise, apply gravity as we did before
        Yvelocity.y += gravity * Time.deltaTime;
        if (isGrounded)
        {
            Yvelocity.y = Mathf.Clamp(Yvelocity.y, maxGravity, 999);
        }
        
    }

    
}
