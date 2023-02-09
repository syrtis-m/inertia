using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        // Link camera position to the camera holder object
        transform.position = cameraPosition.position;
    }
}
