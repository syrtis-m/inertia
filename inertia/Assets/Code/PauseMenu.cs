using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isGamePaused = false;
    public bool isGameFinished = false;
    public GameObject pauseMenu;

    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
    }

        // Update is called once per frame
        void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isGamePaused && !isGameFinished)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (isGamePaused && !isGameFinished)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RestartScene();
            }
        }

        if (isGamePaused && !isGameFinished)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LoadMainMenu();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
