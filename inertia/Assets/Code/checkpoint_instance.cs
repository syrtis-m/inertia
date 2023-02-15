using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint_instance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {//sets the checkpoint spawn point to itself when touched.
        if (other.GetComponent<PlayerMovement>())
        {
            checkpoints.instance.SetCheckpoint(this.name);
        }
    }
}
