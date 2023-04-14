using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minX = -60f;
    public float maxX = 60f;

    public Camera cam;

    float rotY = 0f;
    float rotX = 0f;
    

    PlayerMovement movementScript;
    Mind instance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        movementScript = GetComponent<PlayerMovement>();
        instance = Mind.instance;
    }

    void Update()
    {
        if(!PauseMenu.isGamePaused)
        {
            rotY += Input.GetAxis("Mouse X") * instance.sensitivity;
            rotX += Input.GetAxis("Mouse Y") * instance.sensitivity;

            rotX = Mathf.Clamp(rotX, minX, maxX);

            transform.localEulerAngles = new Vector3(0, rotY, 0);
            cam.transform.localEulerAngles = new Vector3(-rotX, 0, movementScript.tilt);
        }
    }
}
