using UnityEngine;
using TMPro;
using System.Collections;

public class ResourceUI : MonoBehaviour
{

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text killsText;
    private int gold = 0;
    private int kills = 0;

    private int kills2 = 1;
    private int gold2 = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goldText.text = "Gold: " + gold;
        killsText.text = "Kills: " + kills;
        StartCoroutine(UpdateTest(2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(UpdateTest(2.0f));
    }


    // Call these functions when you want to update these resources.
    public void UpdateGold(int newGold)
    {
        gold += newGold;
    }
    public void UpdateKills(int newKills)
    {
        kills += newKills;
    }

    private IEnumerator UpdateTest(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        UpdateGold(gold2);
        UpdateKills(kills2);
        goldText.text = "Gold: " + gold;
        killsText.text = "Kills: " + kills;
        
    }
}
