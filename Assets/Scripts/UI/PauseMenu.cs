using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Animator pauseMenuAnimation;
    public bool paused;

    public void ResetScene()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Update()
    {
        // pause the game and open pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false)
            {
                Time.timeScale = 0.0f;
                pauseMenu.SetActive(true);

                paused = true;
            }
            else if (paused == true)
            {
                Time.timeScale = 1.0f;
                pauseMenu.SetActive(false);

                paused = false;
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);

        paused = false;
    }

    public void MainMenu()
    {
        pauseMenuAnimation.SetBool("Exit", true);

        Time.timeScale = 1.0f;

        Invoke("BackToMainMenu", 1.0f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
