using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Author: [Ruffner, Kaylie]
 * Date of Creation: [2-2-2026]
 * Summary: [This script focuses on collecting information on the main menu.]
 */

public class MainMenuManager: MonoBehaviour
{
    public static MainMenuManager _;
    [SerializeField] private bool _debugMode;
    public enum MainMenuButtons { play, howtoplay, tutorial, quit, back };
    [SerializeField] private int switchScene;

    // this void will let the developers know if there is an error when it comes to the mainmenumanager
    public void Awake()
    {
       if (_ == null)
        {
            _ = this;
        }

       else
        {
            Debug.Log("There are more than one MainMenuManager's in this scene.");
        }
    }

    // this void focuses on when the user actually clicks on the button
    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        Debug.Log("Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            // this works on the buttons that are implimented and have the quit button functional
            case MainMenuButtons.play:
                ButtonClicked();
                break;
            case MainMenuButtons.tutorial:
                ButtonClicked();
                break;
            case MainMenuButtons.howtoplay:
                ButtonClicked();
                break;
            case MainMenuButtons.quit:
                QuitGame();
                break;
            case MainMenuButtons.back:
                ButtonClicked();
                break;
            default:
                Debug.Log("Button clicked was not implimented into the MainMenuManager");
                break;
        }
    }


    // this void will show the debug message if it is called.
    private void DebugMessage(string message)
    {
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }

    // this void switches the scene when the player clicks on the button
    public void ButtonClicked()
    {
        SceneManager.LoadScene(switchScene);
    }

    // this void works on the quit button, where if you hit quit in editor it exits out of playmode
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
