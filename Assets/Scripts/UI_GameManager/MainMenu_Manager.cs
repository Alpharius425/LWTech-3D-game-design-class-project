using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
    /**
     * This script handles some basic functions in using the main menu.
     * These functions will execute when called via UI Buttons.
     * -Grant Hargraves
     */
    //=========================FUNCTIONS=========================
    public void startGame()
    {
        SceneManager.LoadScene(0); //starts the gameplay scene
    }

    public void endGame()
    {
        Application.Quit(); //closes the game window
    }
}
