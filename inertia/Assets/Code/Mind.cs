using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mind : MonoBehaviour
{
    public static Mind instance;
    
    public int newGameScene;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void StartGame(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
