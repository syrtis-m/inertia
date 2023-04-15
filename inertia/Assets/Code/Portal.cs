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
            DisplayTime();
            pauseMenuScript.PauseGame();
            pauseMenuScript.isGameFinished = true;
            pauseMenu.SetActive(false);
            UICanvas.SetActive(false);
            winMenu.SetActive(true);
        }
    }


    void DisplayTime()
    {
        int time = (int)stopWatch.time;
        if(time < 60)
        {
            timeText.text = "Time: " + time;
        }
        else
        {
            int minute = time / 60;
            if ((time % 60) < 10)
            {
                timeText.text = "Time: " + minute + ":0" + (time % 60);
            }
            else
            {
                timeText.text = "Time: " + minute + ":" + (time % 60);
            }
        }
    }
}
        