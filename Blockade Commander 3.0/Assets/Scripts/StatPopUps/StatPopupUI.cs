using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class StatPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackText;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] TauntTower tauntRef;

    public int health = 0;
    public int range = 0;
    public int attack = 0;
    public void ShowStats()
    {
        health = tauntRef.health;
        range = tauntRef.range;
        attack = tauntRef.dmg;
        healthText.text = "Health: " + health;
        rangeText.text = "Range: " + range;
        attackText.text = "Attack: " + attack;

        gameObject.SetActive(true);
        StartCoroutine(FadeAndMove());
    }

    private IEnumerator FadeAndMove()
    {
        canvasGroup.alpha = 0;
        Vector2 startPos = new Vector2(0, -50);
        Vector2 endPos = Vector2.zero;
        rectTransform.anchoredPosition = startPos;

        float duration = 2.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentage = elapsed / duration;

            //making the popup fade in and out
            canvasGroup.alpha = Mathf.Lerp(1, 0, percentage);
            if (percentage > 0.8f) canvasGroup.alpha = Mathf.Lerp(1, 0, (percentage - 0.8f) / 0.2f);

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, percentage);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
