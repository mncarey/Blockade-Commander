using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class FloatingHealthBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    //[SerializeField] private Transform target;
    //[SerializeField] private Vector3 offset;

    private void Awake()
    {
        camera = Camera.main;
       
    }
    public void UpdateHealthBar(float currentvalue, float maxValue)
    {
        slider.value = currentvalue / maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        
        
    }
}
