using System.Collections;
using UnityEngine;
using TMPro;

public class TextFader : MonoBehaviour
{
    public TMP_Text tmpText;
    public float fadeDuration = 1f;

    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(1f, 0f));
    }

    public void FadeInThenOut(float holdTime = 1f)
    {
        StartCoroutine(FadeInOutCoroutine(holdTime));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = tmpText.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            tmpText.color = color;
            yield return null;
        }

        color.a = endAlpha;
        tmpText.color = color;
    }

    private IEnumerator FadeInOutCoroutine(float holdTime)
    {
        yield return FadeCoroutine(0f, 1f);
        yield return new WaitForSeconds(holdTime);
        yield return FadeCoroutine(1f, 0f);
    }
}