using TMPro;
using UnityEngine;

public class UpgradesText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackText;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] private ResourceUI resourceRef;

    private Coroutine currentRoutine;

    public void ShowUpgrades(int goldCost)
    {
        healthText.text = "Health + 1: " + goldCost + " gold";
        rangeText.text = "Range + 1: " + goldCost + " gold";
        attackText.text = "Attack + 1: " + goldCost + " gold";

        gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        // currentRoutine = StartCoroutine(FadeAndMove());
    }
}
