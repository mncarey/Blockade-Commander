using UnityEngine;

public class IncreaseDifficulty : MonoBehaviour
{

    public float multiplier = 1f;
    public IncreaseDifficulty Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }

    public void IncreaseDiff()
    {
        multiplier += 0.2f;
    }
}
