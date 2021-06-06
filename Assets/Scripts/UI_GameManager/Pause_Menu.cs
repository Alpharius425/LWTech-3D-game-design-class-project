using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_Menu : MonoBehaviour
{
    /**
     * This script handles methods and variables used to operate the pause menu in-game
     * -Grant Hargraves and Mikey Petersen
     */
    //=========================FIELDS=========================
    [Header("Buttons")]
    [SerializeField] Button resumeButton; //references to the pause screen buttons so listeners can be added.
    [SerializeField] Button controlsButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button quitButton;
    //-----
    [Header("Other")]
    [SerializeField] GameObject pausePanel; // our panel with all our buttons and text
    [SerializeField] GameObject controlPanel; //the panel containing our controls text
    public bool pause = false; // checks if we are paused or not
    private bool controlsUp; //keeps track of whether the controls panel is up or not
    public int mainMenuScene; // saves the index of our main menu scene
    [SerializeField] Player_Stats myStats;
    //========================BASIC METHODS=========================
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //a futile attempt to keep the cursor within our mortal control
        resumeButton.onClick.AddListener(Resume); //add proper listeners to the pause buttons
        controlsButton.onClick.AddListener(ToggleControls);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(Quit);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) //when hitting the escape key, it will either pause or unpause
        {
            if (pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    //========================CUSTOM METHODS=========================
    public void Quit() // quits the application. only works in builds!!!
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Resume() // unpauses our game
    {
        pausePanel.SetActive(false); //turns off the pause menu
        Time.timeScale = 1; //sets time to normal
        pause = false;
        Cursor.lockState = CursorLockMode.Locked; //locks the cursor to the center of the screen
        myStats.ResumeCameraControl();
    }

    private void ToggleControls() //turns the visibility of the controls panel on or off
    {
        if(controlsUp)
        {
            controlPanel.SetActive(false);
            controlsUp = false;
        }
        else
        {
            controlPanel.SetActive(true);
            controlsUp = true;
        }
    }

    public void Pause() // pauses our game and activates the menu
    {
        pausePanel.SetActive(true); //turns on the pause menu
        Time.timeScale = 0; //pauses time
        pause = true;
        Cursor.lockState = CursorLockMode.None; //frees the cursor to be used in menus
    }

    public void MainMenu() // takes us back to the main menu
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
