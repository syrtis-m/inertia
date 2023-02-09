using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoints : MonoBehaviour
{
    public static checkpoints instance; //singleton pattern
    private Dictionary<string, GameObject> spawnpoints;
    public string currentSpawn;

    private void Awake()
    {
        instance = this;
        spawnpoints = new Dictionary<string, GameObject>();

        var children_components = GetComponentsInChildren<BoxCollider>();
        foreach (var child in children_components)
        {
            //check if they have a checkpoint_instance script.
            spawnpoints.Add(child.name, child.gameObject);
            Debug.Log("init checkpoint: " + child.name);
        }
    }

    public void Respawn()
    {
        PlayerMovement.instance.GetComponent<CharacterController>().enabled = false;
        //move the player to checkpoint.
        PlayerMovement.instance.transform.SetPositionAndRotation(spawnpoints[currentSpawn].transform.position, Quaternion.Euler(Vector3.zero));
        //zero the player's movement/speed/velocity
        PlayerMovement.instance.ResetPlayer(); //reset values that change moment to moment
        PlayerMovement.instance.GetComponent<CharacterController>().enabled = true;

    }

    public void SetCheckpoint(string checkpoint)
    {//potential thing here: visiting a past checkpoint will set that checkpoint to your new spawnpoint. allows for hub-and-spoke level design
        currentSpawn = checkpoint;
        Debug.Log("checkpoint set to: " + checkpoint);
    }
    
    
    
    
}
