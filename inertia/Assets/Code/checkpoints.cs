using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkpoints : MonoBehaviour
{
    public static checkpoints instance; //singleton pattern

    [Tooltip("set to canvas->screenwipeImg")]
    public Image screenwipeImg;
    [Tooltip("float of how long a screenwipe lasts in seconds, non-negative")]
    public float screenwipeTime = 0.1f;
    
    private Color _startColor = new Color(0, 0, 0, 0);
    private Color _targetColor = Color.black;
    
    private Dictionary<string, GameObject> spawnpoints;
    
    [Tooltip("player will spawn/respawn at whatever checkpoint this is set to.")]
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

        //set the first checkpoint to the 
        if (currentSpawn != String.Empty)
        {
            SetCheckpoint(currentSpawn);
            Respawn();
        }
    }

    public void Respawn()
    {
        StartCoroutine(ScreenWipe());
        
    }

    IEnumerator ScreenWipe()
    {//wipe the screen

        var time = screenwipeTime / 6;
        var interval = time / 10;
        //lerp color between two values.
        Color c = _startColor;
        
        
        //fade to black
        for (float alpha = 0f; alpha <= 1; alpha += 0.1f)
        {
            c.a = alpha;
            screenwipeImg.color = c;
            yield return new WaitForSeconds(interval);
        }
        
        //reset player
        PlayerMovement.instance.GetComponent<CharacterController>().enabled = false;
        //move the player to checkpoint.
        PlayerMovement.instance.transform.SetPositionAndRotation(spawnpoints[currentSpawn].transform.position, Quaternion.Euler(Vector3.zero));
        //zero the player's movement/speed/velocity
        PlayerMovement.instance.ResetPlayer(); //reset values that change moment to moment
        PlayerMovement.instance.GetComponent<CharacterController>().enabled = true;

        screenwipeImg.color = _targetColor;
        //wait at black
        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(interval);
        }

        c = _targetColor;
        //fade back in
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            screenwipeImg.color = c;
            yield return new WaitForSeconds(interval);
        }

        screenwipeImg.color = _startColor;
    }

    public void SetCheckpoint(string checkpoint)
    {//potential thing here: visiting a past checkpoint will set that checkpoint to your new spawnpoint. allows for hub-and-spoke level design
        currentSpawn = checkpoint;
        Debug.Log("checkpoint set to: " + checkpoint);
    }
    
    
    
    
}
