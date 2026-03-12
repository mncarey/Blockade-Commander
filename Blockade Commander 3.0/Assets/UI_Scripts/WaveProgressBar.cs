using UnityEngine;
using UnityEngine.UI;

public class WaveProgressBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;

    public Wave_Spawner_BasicEnemy waveRef;
    public StartWaveButton startWaveButtonRef;
    public int enemiesAlive;
    public int enemiesSpawned;

    private void Awake()
    {
        camera = Camera.main;

    }
    public void UpdateHealthBar()
    {
        if (waveRef == null || startWaveButtonRef == null) return;

        enemiesAlive = waveRef.enemiesAlive;
        enemiesSpawned = startWaveButtonRef.maxEnemies;

        if (enemiesSpawned > 0)
            slider.value = 1f - (float)enemiesAlive / enemiesSpawned;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.rotation = camera.transform.rotation;

    }
}
