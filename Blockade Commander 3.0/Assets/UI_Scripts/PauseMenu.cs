using UnityEngine;
/*
 * Author: [Ruffner, Kaylie]
 * Date: [2-17-2026]
 * Summar: [This script handles the pause menu]
 */
public class PauseMenu : MonoBehaviour
{
    // -* game objects for the how to pause menu *- \\
    public GameObject pauseMenuUI;
    public GameObject instructionsMenu;
    public GameObject pauseButton;

    // -* game objects for each instructions menu *- \\
    public GameObject instructionsOne;
    public GameObject instructionsTwo;
    public GameObject instructionsThree;
    public GameObject instructionsFour;
    public GameObject instructionsFive;

    private bool isPaused = false;

    // this function activates the pause menu canvas, checks that its paused to be true and freezes the game
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        pauseButton.SetActive(false);
    }

    // this function deactivates the pause menu canvas, checks that the pause is false and resumes the game
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        pauseButton.SetActive(true);
    }

    // this function calls the other functions and checks of the game is paused or not
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    // this function makes the How to Play menu activate when the player clicks on the how to play menu button and deactivates the pause menu
    public void HowtoPlayMenuActive()
    {
        instructionsMenu.SetActive(true);
        pauseMenuUI.SetActive(false);
        isPaused = true;
        pauseButton.SetActive(false);
    }

    // this function deactivates the how to play menu and reactivates the pause menu
    public void HowtoPlayMenuInActive()
    {
        instructionsMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
        isPaused = true;
        pauseButton.SetActive(false);
    }

    // ---- these functions handle turning on and off each instruction image when the player hits next and turns it back on when the player hits back ---- \\

    // first instructions
    // -* active *-
    public void OneInstructionsActive()
    {
        instructionsOne.SetActive(true);
        instructionsMenu.SetActive(false);
        isPaused = true;
    }
    // -* inactive *-
    public void OneInstructionsInActive()
    {
        instructionsOne.SetActive(false);
        isPaused = true;
    }

    // second instructions
    // -* active *-
    public void TwoInstructionsActive()
    {
        instructionsTwo.SetActive(true);
        isPaused = true;
    }
    // -* inactive *-
    public void TwoInstructionsInActive()
    {
        instructionsTwo.SetActive(false);
        isPaused = true;
    }

    // third instructions
    // -* active *-
    public void ThreeInstructionsActive()
    {
        instructionsThree.SetActive(true);
        isPaused = true;
    }
    // -* inactive *-
    public void ThreeInstructionsInActive()
    {
        instructionsThree.SetActive(false);
        isPaused = true;
    }

    // fourth instructions
    // -* active *-
    public void FourInstructionsActive()
    {
        instructionsFour.SetActive(true);
        isPaused = true;
    }
    // -* inactive *-
    public void FourInstructionsInActive()
    {
        instructionsFour.SetActive(false);
        isPaused = true;
    }

    // fifth instructions
    // -* active *-
    public void FiveInstructionsActive()
    {
        instructionsFive.SetActive(true);
        isPaused = true;
    }
    // -* inactive *-
    public void FiveInstructionsInActive()
    {
        instructionsFive.SetActive(false);
        isPaused = true;
    }
}
