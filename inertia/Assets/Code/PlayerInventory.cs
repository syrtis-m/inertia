using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public KeysUI keysUI;
    
    public List<int> keys;

    private void Awake()
    {
        instance = this;
        keys = new List<int>();
    }

    public void RegisterKey(int key)
    {
        keys.Add(key);
        keysUI.CollectKey();
    }

    public bool CheckKey(int key)
    {
        return keys.Contains(key);
    }
}
