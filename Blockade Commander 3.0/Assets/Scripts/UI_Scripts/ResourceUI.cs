using UnityEngine;
using TMPro;
using System.Collections;

public class ResourceUI : MonoBehaviour
{

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text killsText;
    private int gold = 0;
    private int kills = 0;

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
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Call these functions when you want to update these resources.
    public void UpdateGold(int newGold)
    {
        Debug.Log("Gold now equals: " + gold);
        Debug.Log("Updating text object: " + goldText.gameObject.name);
        gold += newGold;
        goldText.text = "Gold: " + gold;
    }
    public void UpdateKills(int newKills)
    {
        kills += newKills;
        killsText.text = "Kills: " + kills;
    }

    
}
