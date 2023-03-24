using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mind : MonoBehaviour
{
    public static Mind instance;

    public float sensitivity;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
