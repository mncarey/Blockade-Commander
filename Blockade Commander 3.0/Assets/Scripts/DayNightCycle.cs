using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Length of a full day in minutes")]
    public float dayLengthInMinutes = 5f;

    [Range(0f, 1f)]
    public float currentTime = 0.25f;//start during the day

    [Header("References")]
    public Light sunLight;
    public Light moonLight;

    [Header("Sun Rotation")]
    public Vector3 sunRotationOffset = new Vector3(-90f, 0f, 0f);

    [Header("Gradient Color")]
    public Gradient ambientColor;

    void Update()
    {
        //Advance time
        currentTime += Time.deltaTime / (dayLengthInMinutes * 60f);

        //Loop forever
        if (currentTime >= 1f)
            currentTime = 0f;

        UpdateSun();
        //UpdateMoon();
    }

    void UpdateSun()
    {
        //Convert normalized time into a 360 rotation
        float sunAngle = currentTime * 360f;

        sunLight.transform.rotation =
            Quaternion.Euler(sunRotationOffset + new Vector3(sunAngle, 0f, 0f));

        //Sun height calculation
        float dot = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        //Intensity
        sunLight.intensity = Mathf.Clamp01(dot) * 3f;


        RenderSettings.ambientLight = ambientColor.Evaluate(currentTime);

    }

   
}
