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

    // -* game objects for each instructions menu *- \\
    public GameObject instructionsOne;
    public GameObject instructionsTwo;
    public GameObject instructionsThree;
    public GameObject instructionsFour;
    public GameObject instructionsFive;

    public GameObject mainMenuButton;


    // ---- these functions handle turning on and off each instruction image when the player hits next and turns it back on when the player hits back ---- \\

    // first instructions
    // -* active *-
    public void OneInstructionsActive()
    {
        instructionsOne.SetActive(true);
        mainMenu.SetActive(false);
        mainMenuButton.SetActive(true);
    }
    // -* inactive *-
    public void OneInstructionsInActive()
    {
        instructionsOne.SetActive(false);
        mainMenu.SetActive(true);
        mainMenuButton.SetActive(false);
    }
    
    // second instructions
    // -* active *-
    public void TwoInstructionsActive()
    {
        instructionsTwo.SetActive(true);
        mainMenuButton.SetActive(true);
    }
    // -* inactive *-
    public void TwoInstructionsInActive()
    {
        instructionsTwo.SetActive(false);
        mainMenuButton.SetActive(true);
    }

    // third instructions
    // -* active *-
    public void ThreeInstructionsActive()
    {
        instructionsThree.SetActive(true);
        mainMenuButton.SetActive(true);
    }
    // -* inactive *-
    public void ThreeInstructionsInActive()
    {
        instructionsThree.SetActive(false);
        mainMenuButton.SetActive(true);
    }

    // fourth instructions
    // -* active *-
    public void FourInstructionsActive()
    {
        instructionsFour.SetActive(true);
        mainMenuButton.SetActive(true);
    }
    // -* inactive *-
    public void FourInstructionsInActive()
    {
        instructionsFour.SetActive(false);
        mainMenuButton.SetActive(true);
    }

    // fifth instructions
    // -* active *-
    public void FiveInstructionsActive()
    {
        instructionsFive.SetActive(true);
        mainMenuButton.SetActive(true);
    }
    // -* inactive *-
    public void FiveInstructionsInActive()
    {
        instructionsFive.SetActive(false);

    }
}
