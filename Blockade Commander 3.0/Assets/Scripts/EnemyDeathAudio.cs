using UnityEngine;

public class EnemyDeathAudio : MonoBehaviour
{
    public static EnemyDeathAudio Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip enemyDeathClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayEnemyDeath()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(enemyDeathClip);
        }
    }
}
