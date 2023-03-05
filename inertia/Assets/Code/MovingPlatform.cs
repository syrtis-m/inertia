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
        if (dist < .005f)
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
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement.instance.externalMovement = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        //draw a line between points.
        Gizmos.color = Color.red;
        for (int i = 1; i < points.Count; i++)
        {
            Gizmos.DrawLine(points[i - 1].position, points[i].position);
            if (i == points.Count - 1)
            {
                Gizmos.DrawLine(points[i].position,points[0].position);
            }
        }
    }
}
