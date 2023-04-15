using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysUI : MonoBehaviour
{
    public GameObject key1;
    public GameObject key2;
    public GameObject key3;

    public bool key1Collected = false;
    public bool key2Collected = false;
    public bool key3Collected = false;

    public void CollectKey()
    {
        if(!key1Collected)
        {
            key1Collected = true;
            key1.SetActive(true);
        }
        else if(!key2Collected)
        {
            key2Collected = true;
            key2.SetActive(true);
        }
        else
        {
            key3Collected = true;
            key3.SetActive(true);
        }
    }
}
