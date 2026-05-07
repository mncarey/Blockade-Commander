using TMPro;
using UnityEngine;

public class UpgradesText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackText;

    [SerializeField] private TextMeshProUGUI healthLvlText;
    [SerializeField] private TextMeshProUGUI rangeLvlText;
    [SerializeField] private TextMeshProUGUI attackLvlText;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] private ResourceUI resourceRef;

    private Coroutine currentRoutine;

    public void ShowUpgrades(int goldCost)
    {
        healthText.text = "Health + 1: " + (goldCost) + " gold";
        if(rangeText.text != null)
        {
            rangeText.text = "Range + 1: " + (goldCost) + " gold";
        }
        
        if(attackText.text != null)
        {
            attackText.text = "Attack + 1: " + (goldCost) + " gold";
        }
       

        gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
    }

    public void ShowUpgradeLvl(int upgradeLvl)
    {
        healthLvlText.text = "Lvl " + upgradeLvl;
        if(rangeLvlText != null)
        {
            rangeLvlText.text = "Lvl " + upgradeLvl;
        }

        if (attackLvlText.text != null)
        {
            attackLvlText.text = "Lvl " + upgradeLvl;
        }
       
    }
}
