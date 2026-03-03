using UnityEngine;

public class Blinking : MonoBehaviour
{
    public GameObject targetObject;

    public float repeatTime = 0.2f;
    public float bigRepeatTime = 10f;
    public float blinkDuration = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("StartBlinking", bigRepeatTime, bigRepeatTime);
    }

    void ChangeStateofGameObject()
    {
        targetObject.SetActive(!targetObject.activeInHierarchy); //set to the opposite of current state
    }

    void StartBlinking()
    {
        InvokeRepeating("ChangeStateofGameObject", 0f, repeatTime);
        Invoke("StopBlinking", blinkDuration);
    }

    void StopBlinking()
    {
        CancelInvoke("ChangeStateofGameObject");
        targetObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
