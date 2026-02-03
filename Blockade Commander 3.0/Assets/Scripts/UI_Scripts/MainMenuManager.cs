using UnityEngine;

/*
 * Author: [Ruffner, Kaylie]
 * Date of Creation: [2-2-2026]
 * Summary: [This script focuses on collecting information on the main menu.]
 */

public class MainMenuManager: MonoBehaviour
{
    public static MainMenuManager _;
    [SerializeField] private bool _debugMode;
    public enum MainMenuButtons { play, howtoplay, tutorial, quit };


    public void Awake()
    {
       if (_ == null)
        {
            _ = this;
        }

       else
        {
            Debug.LogError("There are more than one MainMenuManager's in this scene.");
        }
    }

    // this void focuses on a debug log that will show when the player "clicks on a button"
    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        Debug.Log("Button Clicked: " + buttonClicked.ToString());
    }


    // this void will show the debug message if it is called.
    private void DebugMessage(string message)
    {
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }
}
