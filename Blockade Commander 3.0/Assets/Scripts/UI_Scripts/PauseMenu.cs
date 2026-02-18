using UnityEngine;
/*
 * Author: [Ruffner, Kaylie]
 * Date: [2-17-2026]
 * Summar: [This script handles the pause menu]
 */
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject instructionsMenu;
    public GameObject pauseButton;

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
}
