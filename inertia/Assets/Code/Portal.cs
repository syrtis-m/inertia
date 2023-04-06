using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    
    public int sceneToLoad = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            Mind.instance.EnterScene(sceneToLoad);
        }
    }
}
        