using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour
{

    public GameObject winMenu;
    public GameObject UICanvas;
    public GameObject pauseMenu;
    public PauseMenu pauseMenuScript;
    public StopWatch stopWatch;
    public TMP_Text timeText;
    public TMP_Text bestTimeText;

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
        int index = SceneManager.GetActiveScene().buildIndex;
        if (!PlayerPrefs.HasKey("Scene" + index))
        {
            PlayerPrefs.SetInt("Scene" + index, time);
        }
        else
        {
            int currentBestScore = PlayerPrefs.GetInt("Scene" + index);
            if (time < currentBestScore)
            {
                PlayerPrefs.SetInt("Scene" + index, time);
            }
        }
        int bestTime = PlayerPrefs.GetInt("Scene" + index);

        if (time < 60)
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

        if (bestTime < 60)
        {
            bestTimeText.text = "Best Time: " + bestTime;
        }
        else
        {
            int bestMinute = bestTime / 60;
            if ((bestTime % 60) < 10)
            {
                bestTimeText.text = "Best Time: " + bestMinute + ":0" + (bestTime % 60);
            }
            else
            {
                bestTimeText.text = "Best Time: " + bestMinute + ":" + (bestTime % 60);
            }
        }
    }
}
        