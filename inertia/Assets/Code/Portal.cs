using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public GameObject winMenu;
    public GameObject UICanvas;
    public GameObject pauseMenu;
    public PauseMenu pauseMenuScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            pauseMenuScript.PauseGame();
            pauseMenuScript.isGameFinished = true;
            pauseMenu.SetActive(false);
            UICanvas.SetActive(false);
            winMenu.SetActive(true);
        }
    }
}
        