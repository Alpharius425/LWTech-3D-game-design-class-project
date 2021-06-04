using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    // put on the canvas on the player prefab

    [SerializeField] GameObject pausePanel; // our panel with all our buttons and text
    public bool pause = false; // checks if we are paused or not
    [SerializeField] GameObject tutorialText; // text for tutorial
    [SerializeField] int mainMenuScene; // saves the index of our main menu scene

    public void Quit() // quits the application. only works in builds!!!
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Resume() // unpauses our game and turns off the menu
    {
        Time.timeScale = 1;
        tutorialText.SetActive(false);
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // locks the cursor
        pause = false;
    }

    public void Pause() // pauses our game and activates the menu
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined; // unlocks the cursor
        pause = true;
    }

    public void ToggleTutorial() // turns the tutorial text on and off in the menu
    {
        if(tutorialText.activeInHierarchy == true)
        {
            tutorialText.SetActive(false);
        }
        else
        {
            tutorialText.SetActive(true);
        }
    }

    public void MainMenu() // takes us back to the main menu
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
