using UnityEngine;
using UnityEngine.UI;

public class WaveProgressBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;

    public Wave_Spawner_BasicEnemy waveRef;
    public StartWaveButton startWaveButtonRef;


    private void Awake()
    {
        camera = Camera.main;

    }
    public void UpdateHealthBar(float currentvalue, float maxValue)
    {
        slider.value = waveRef.enemiesAlive / startWaveButtonRef.maxEnemies;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.rotation = camera.transform.rotation;


    }
}
