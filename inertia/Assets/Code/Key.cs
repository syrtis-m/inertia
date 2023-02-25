using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : AbstractInteractable
{
    public int KeyID;
    
    public override void Interact()
    {
        PlayerInventory.instance.RegisterKey(KeyID);
        Destroy(gameObject);
    }
}
