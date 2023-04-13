using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Portal : MonoBehaviour
{

    public GameObject winMenu;
    public GameObject UICanvas;
    public GameObject pauseMenu;
    public PauseMenu pauseMenuScript;
    public StopWatch stopWatch;
    public TMP_Text timeText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            timeText.text = "Time: " + (int)stopWatch.time;
            pauseMenuScript.PauseGame();
            pauseMenuScript.isGameFinished = true;
            pauseMenu.SetActive(false);
            UICanvas.SetActive(false);
            winMenu.SetActive(true);
        }
    }
}
        