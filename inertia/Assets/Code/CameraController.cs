using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minX = -60f;
    public float maxX = 60f;

    public Camera cam;

    private float rotY = 0f;
    private float rotX = 0f;

    //public Vector3 initialRotation = Vector3.zero;
    

    PlayerMovement movementScript;
    Mind instance;

    private void Awake()
    {
        rotY = gameObject.transform.rotation.eulerAngles.y; //set camera rotation to the transform rotation
    }

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
