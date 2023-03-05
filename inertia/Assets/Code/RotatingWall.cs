using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{//this script rotates an object by x,y,z speed
    [Tooltip("x-axis rotation speed between -360 and 360. 0 for no rotation")]
     public float x_speed;
     [Tooltip("y-axis rotation speed between -360 and 360. 0 for no rotation")]
     public float y_speed;
     [Tooltip("z-axis rotation speed between -360 and 360. 0 for no rotation")]
     public float z_speed;

    
    private void FixedUpdate()
    {
        transform.Rotate(x_speed*Time.deltaTime,y_speed*Time.deltaTime,z_speed*Time.deltaTime);
    }
}
