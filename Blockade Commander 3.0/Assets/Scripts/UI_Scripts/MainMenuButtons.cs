using UnityEngine;
/*
 * Author: [Ruffner, Kaylie]
 * Date of Creation: [2-3-2026]
 * Summary: [This script focuses on the functionality of the buttons on the main menu.]
 */

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] MainMenuManager.MainMenuButtons _buttonType;
    

    public void ButtonClicked()
    {
        MainMenuManager._.MainMenuButtonClicked(_buttonType);
    }
}
