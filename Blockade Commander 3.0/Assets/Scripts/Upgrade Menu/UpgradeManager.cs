using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    
    public int upgradeLvlTaunt = 0;
    public int upgradeLvlCannon = 0;
    public int upgradeLvlWall = 0;

    public int upgradeLvl = 3;
    

    public int tauntPrice = 30;//base price
    public int wallPrice = 30;//base price
    public int cannonPrice = 30;//base price

    public AscensionManager ascensionRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
