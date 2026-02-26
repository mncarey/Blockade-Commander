using UnityEngine;
/*
 * Author: [Ruffner, Kaylie]
 * Date: [2-18-2026]
 * Summar: [This script handles the switching between main menu and how to play menu, as well as switching between the sequence of the gameplay.]
 */

public class HPTMenu : MonoBehaviour
{
    // -* game objects for the how to play menu *- \\
    public GameObject mainMenu;
    public GameObject instructionsMenu;

    // -* game objects for each instructions menu *- \\
    public GameObject instructionsOne;
    public GameObject instructionsTwo;
    public GameObject instructionsThree;
    public GameObject instructionsFour;
    public GameObject instructionsFive;

    // ---- these functions handle the active and inactive for the how to play menu after the player hits said button from the main menu canvas ---- \\

    // this function activates the how to play menu when the player presses the button assigned for it
    public void HowtoPlayMenuActive()
    {
        instructionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    // this function deactivates the menu when the player goes back to the main menu
    public void HowToPlayInActive()
    {
        instructionsMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    // ---- these functions handle turning on and off each instruction image when the player hits next and turns it back on when the player hits back ---- \\

    // first instructions
    // -* active *-
    public void OneInstructionsActive()
    {
        instructionsOne.SetActive(true);
        instructionsMenu.SetActive(false);
    }
    // -* inactive *-
    public void OneInstructionsInActive()
    {
        instructionsOne.SetActive(false);
    }
    
    // second instructions
    // -* active *-
    public void TwoInstructionsActive()
    {
        instructionsTwo.SetActive(true);
    }
    // -* inactive *-
    public void TwoInstructionsInActive()
    {
        instructionsTwo.SetActive(false);
    }

    // third instructions
    // -* active *-
    public void ThreeInstructionsActive()
    {
        instructionsThree.SetActive(true);
    }
    // -* inactive *-
    public void ThreeInstructionsInActive()
    {
        instructionsThree.SetActive(false);
    }

    // fourth instructions
    // -* active *-
    public void FourInstructionsActive()
    {
        instructionsFour.SetActive(true);
    }
    // -* inactive *-
    public void FourInstructionsInActive()
    {
        instructionsFour.SetActive(false);
    }

    // fifth instructions
    // -* active *-
    public void FiveInstructionsActive()
    {
        instructionsFive.SetActive(true);
    }
    // -* inactive *-
    public void FiveInstructionsInActive()
    {
        instructionsFive.SetActive(false);
    }
}
