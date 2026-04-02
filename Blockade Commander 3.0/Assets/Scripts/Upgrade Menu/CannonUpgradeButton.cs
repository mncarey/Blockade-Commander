using UnityEngine;

public class CannonUpgradeButton : MonoBehaviour
{
    public GameObject cannonUpgradeMenuRef;
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
        cannonUpgradeMenuRef.SetActive(true);
    }
}
