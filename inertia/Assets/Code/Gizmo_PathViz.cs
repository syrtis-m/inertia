using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementPathViz : MonoBehaviour
{
    [Tooltip("a waypoint is logged every N frames")]
    public int waypointLoggingFrequency;
    public List<Vector3> waypoints;

    private void Start()
    {
        waypoints = new List<Vector3>();
    }

    private void Update()
    {
        if (Time.frameCount % waypointLoggingFrequency == 0)
        {
            //This will be only executed each 10 frames
            waypoints.Add(PlayerMovement.instance.transform.position);
        }
        
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            //draw a line between each thing in waypoints

            for (int i = 0; i < waypoints.Count-1; i++)
            {
                Gizmos.DrawLine(waypoints[i],waypoints[i+1]);
            }
        }
    }
}
