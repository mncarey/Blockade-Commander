using UnityEngine;
/*
 * Author: [Ruffner, Kaylie]
 * Date: [2-18-2026]
 * Summar: [This script handles the switching between main menu and how to play menu]
 */

public class HPTMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionsMenu;

    // this void activates the how to play menu when the player presses the button assigned for it
    public void HowtoPlayMenuActive()
    {
        instructionsMenu.SetActive(true);
    }

    // this void deactivates the menu when the player goes back to the main menu

    public void HowToPlayInActive()
    {
        instructionsMenu.SetActive(false);
    }
}
