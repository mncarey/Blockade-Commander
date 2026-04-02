using UnityEngine;

public class TowerUpgradeButton : MonoBehaviour
{
    public GameObject towerUpgradeMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IWasClicked()
    {
        towerUpgradeMenu.SetActive(true);
    }


}
