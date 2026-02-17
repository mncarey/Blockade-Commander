using UnityEngine;

public class FortMenuController : MonoBehaviour


    
{
    public GameObject slotContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearSelection()
    {
        for (int i = 0; i < slotContainer.transform.childCount; i++)
        {

            GameObject slot = slotContainer.transform.GetChild(i).gameObject;
            Debug.Log(slot.name);
        }
    }
}
