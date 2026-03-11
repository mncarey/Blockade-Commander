using UnityEngine;
/*
 * Author: [Ruffner, Kaylie]
 * Date: [3-10-2026]
 * Summar: [This script handles the tutorial scene in the game]
 */

public class TutorialScene : MonoBehaviour
{
    // -* game objects for the rest of the UI *- \\
    public GameObject pauseButton;

    // -* game objects for each instructions menu *- \\
    public GameObject welcomeIntro;
    public GameObject instructionsOne;
    public GameObject instructionsTwo;
    public GameObject instructionsThree;
    public GameObject instructionsFour;
    public GameObject instructionsFive;

    // -* game objects for the pop up assets *- \\
    public GameObject popUpOne;
    public GameObject popUpTwo;
    public GameObject popUpThree;

    public GameObject playGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // sets the main game UI to false
        pauseButton.SetActive(false);
        popUpOne.SetActive(false);
        popUpTwo.SetActive(false);
        popUpThree.SetActive(false);

        // activates the welcome intro
        welcomeIntro.SetActive(true);
    }

    public void WelcomeActive()
    {
        welcomeIntro.SetActive(true);
    }

    public void WelcomeInactive()
    {
        welcomeIntro.SetActive(false);
    }

    // -* pop up one active *- \\
    public void PopUpOneActive()
    {
        welcomeIntro.SetActive(false);
        instructionsOne.SetActive(true);
        popUpOne.SetActive(true);
    }

    // -* pop up one inactive *\\
    public void PopUpOneInactive()
    {
        welcomeIntro.SetActive(true);
        instructionsOne.SetActive(false);
        popUpOne.SetActive(false);
    }

    public void PopUpTwoActive()
    {
        welcomeIntro.SetActive(false);
        instructionsTwo.SetActive(true);
        instructionsOne.SetActive(false);
        popUpOne.SetActive(true);
    }

    public void PopUpTwoInactive()
    {
        welcomeIntro.SetActive(false);
        instructionsTwo.SetActive(false);
        instructionsOne.SetActive(true);
        popUpOne.SetActive(true);
    }

    public void PopUpThreeActive()
    {
        welcomeIntro.SetActive(false);
        instructionsThree.SetActive(true);
        instructionsTwo.SetActive(false);
        popUpOne.SetActive(true);
    }

    public void PopUpThreeInactive()
    {
        welcomeIntro.SetActive(false);
        instructionsThree.SetActive(false);
        instructionsTwo.SetActive(true);
        popUpOne.SetActive(true);
    }

    public void PopUpFourActive()
    {
        welcomeIntro.SetActive(false);
        instructionsFour.SetActive(true);
        instructionsThree.SetActive(false);
        popUpOne.SetActive(true);
        popUpTwo.SetActive(true);
        popUpThree.SetActive(true);
    }

    public void PopUpFourInactive()
    {
        welcomeIntro.SetActive(false);
        instructionsFour.SetActive(false);
        instructionsThree.SetActive(true);
        popUpOne.SetActive(true);
        popUpTwo.SetActive(true);
        popUpThree.SetActive(true);
    }

    public void PopUpFiveActive()
    {
        welcomeIntro.SetActive(false);
        instructionsFive.SetActive(true);
        instructionsFour.SetActive(false);
        popUpOne.SetActive(true);
        popUpTwo.SetActive(true);
        popUpThree.SetActive(true);
    }

    public void PopUpFiveInactive()
    {
        welcomeIntro.SetActive(false);
        instructionsFive.SetActive(false);
        instructionsFour.SetActive(true);
        popUpOne.SetActive(true);
        popUpTwo.SetActive(true);
        popUpThree.SetActive(true);
    }

    public void PlayGameActive()
    {
        welcomeIntro.SetActive(false);
        playGame.SetActive(true);
        instructionsFive.SetActive(false);
    }

    public void PlayGameInactive()
    {
        welcomeIntro.SetActive(false);
        playGame.SetActive(false);
        instructionsFive.SetActive(true);
        popUpOne.SetActive(false);
        popUpTwo.SetActive(false);
        popUpThree.SetActive(false);
    }
}
