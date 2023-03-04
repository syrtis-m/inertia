using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    public List<Transform> points;
    public float speed;
    
    private int currentDestination;
    private Vector3 prevPos;

    private void FixedUpdate()
    {
        var target = points[currentDestination];
        
        var dist = Vector3.Distance(transform.position, target.position);
        if (dist < .05f)
        {
            currentDestination++;
            if (currentDestination >= points.Count)
            {
                currentDestination = 0;
            }
        }

        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    //these make sure the player stays stuck to the platform
    private void OnTriggerStay(Collider other)
    {
        //calculate velocity and direction of object
        var curPos = transform.position;
        PlayerMovement.instance.externalMovement = curPos - prevPos;
        prevPos = curPos;
        Debug.Log("a");
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement.instance.externalMovement = Vector3.zero;
    }
}
