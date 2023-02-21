using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    public int newGameScene;

    public void StartGame()
    {
        SceneManager.LoadScene(newGameScene);
    }
}
