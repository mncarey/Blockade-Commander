using TMPro;
using UnityEngine;

public class UpgradesText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackText;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    private Coroutine currentRoutine;

    public void ShowStats(int health, int range, int attack)
    {
        healthText.text = "Health: " + health;
        rangeText.text = "Range: " + range;
        attackText.text = "Attack: " + attack;

        gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        // currentRoutine = StartCoroutine(FadeAndMove());
    }
}
