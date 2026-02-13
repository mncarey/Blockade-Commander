using Unity.VisualScripting;
using UnityEngine;

public class TauntTower : MonoBehaviour
{
    public int health = 10;

    private void Update()
    {

    }

    public void takeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
