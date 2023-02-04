using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public Transform groundCheck;

    public LayerMask groundMask;
    public LayerMask wallMask;

    Vector3 move;
    Vector3 input;
    Vector3 Yvelocity;
    Vector3 forwardDirection;

    private float speed;

    public float runSpeed;
    public float sprintSpeed;
    public float airSpeedMultiplier;

    float gravity;
    public float normalGravity;
    public float wallRunGravity;
    public float jumpHeight;

    public float wallRunSpeedIncrease;
    public float wallRunSpeedDecrease;

    int jumpCharges;

    bool isSprinting;
    bool isWallRunning;
    bool isGrounded;

    float startHeight;
    Vector3 standingCenter = new Vector3(0, 0, 0);

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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        startHeight = transform.localScale.y;
        jumpCharges = 2;
        normalFov = playerCamera.fieldOfView;
    }

    void IncreaseSpeed(float speedIncrease)
    {
        speed += speedIncrease;
    }

    void DecreaseSpeed(float speedDecrease)
    {
        speed -= speedDecrease * Time.deltaTime;
    }

    void Update()
    {
        HandleInput();
        CheckWallRun();
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
        controller.Move(move * Time.deltaTime);
        CameraEffects();
        ApplyGravity();
    }

    void FixedUpdate()
    {
        CheckGround();
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

    void HandleInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (!isWallRunning)
        {
            input = transform.TransformDirection(input);
            input = Vector3.ClampMagnitude(input, 1f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        if (Input.GetKeyUp(KeyCode.Space) && jumpCharges > 0)
        {
            Jump();
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
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 0.7f, wallMask);
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.7f, wallMask);

        if ((onRightWall || onLeftWall) && !isWallRunning && !isGrounded)
        {
            TestWallRun();
        }
        else if ((!onRightWall && !onLeftWall) && isWallRunning)
        {
            ExitWallRun();
        }
    }

    void GroundedMovement()
    {
        speed = isSprinting ? sprintSpeed : runSpeed;

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
            move += forwardDirection * airSpeedMultiplier;
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
        if (input.z > (forwardDirection.z - 10f) && input.z < (forwardDirection.z + 10f))
        {
            move += forwardDirection;
        }
        else if (input.z < (forwardDirection.z - 10f) || input.z > (forwardDirection.z + 10f))
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
        isWallJumping = true;
        wallJumpTimer = maxWallJumpTimer;
    }

    void Jump()
    {
        if (!isGrounded && !isWallRunning)
        {
            jumpCharges -= 1;
        }
        else if (isWallRunning)
        {
            ExitWallRun();
        }
        Yvelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);
    }

    void ApplyGravity()
    {
        gravity = isWallRunning ? wallRunGravity : normalGravity;
        Yvelocity.y += gravity * Time.deltaTime;
        controller.Move(Yvelocity * Time.deltaTime);
    }
}
