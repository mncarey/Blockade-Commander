using UnityEngine;
using UnityEngine.UI;
public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Camera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = camera.transform.rotation;
    }
}
