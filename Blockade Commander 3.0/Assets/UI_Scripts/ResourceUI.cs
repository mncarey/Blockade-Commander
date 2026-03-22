using UnityEngine;
using TMPro;
using System.Collections;

public class ResourceUI : MonoBehaviour
{

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text fortNumberText;
    public int gold = 0;
    private int kills = 0;
    public int fortNumber = 0;

    public static ResourceUI instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goldText.text = "Gold: " + gold;
        killsText.text = "Kills: " + kills;
        fortNumberText.text = fortNumber + "/4";
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Call these functions when you want to update these resources.
    public void UpdateGold(int newGold)
    {
        
        gold += newGold;
        goldText.text = "Gold: " + gold;
    }
    public void UpdateKills(int newKills)
    {
        kills += newKills;
        killsText.text = "Kills: " + kills;
    }

    public void ResetResources()
    {
        kills = 0;
        gold = 0;
        killsText.text = "Kills: " + kills;
        goldText.text = "Gold: " + gold;
    }

    public void UpdateFortRef(int fortNum)
    {
        fortNumberText.text = fortNum + "/4";
    }
}
