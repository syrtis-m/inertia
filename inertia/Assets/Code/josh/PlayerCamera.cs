using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    // Sensitivity scalars for mouse
    public float sensitivityX;
    public float sensitivityY;

    //
    public Transform playerOrientation;

    // Stores current rotation every update cycle
    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Set cursor to screen center and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse inputs
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensitivityY;

        // Set rotation properly (this is weird, but just how unity works
        xRotation -= mouseY;
        yRotation += mouseX;

        // Make sure you can't look up or down more than 90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate camera and set playerOrientation Transform
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
