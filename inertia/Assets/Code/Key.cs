using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int KeyID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            PlayerInventory.instance.RegisterKey(KeyID);
            Destroy(gameObject);
        }
    }
}
