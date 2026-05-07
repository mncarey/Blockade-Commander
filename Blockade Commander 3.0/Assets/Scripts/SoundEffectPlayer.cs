using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    
    public void PlaySound()
    {
        if(audioSource == null)
        {
            audioSource.Play();
        }
    }
}
