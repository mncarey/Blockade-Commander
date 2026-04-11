using UnityEngine;
using TMPro;
using static FortData;

public class FortUIPlaceTracker : MonoBehaviour
{
    public FortType fortType;
    private PlacingScript placingScriptRef;
    public TMP_Text countText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        placingScriptRef = FindObjectOfType<PlacingScript>();
    }
    void Update()
    {
        int current = placingScriptRef.GetFortCount(fortType);
        countText.text = current + " / " + placingScriptRef.maxPerType;
    }
}
