using Unity.VisualScripting;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        //gets the AudioSource componenton this GameObject (SoundEffects)
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found on SoundEffects object.");
        }
    }

    //method to play the shot sound effect
    public void PlaySound(AudioClip soundToPlay)
    {
        if ((audioSource != null) && (soundToPlay != null))
        {
            audioSource.PlayOneShot(soundToPlay);
        } else
        {
            Debug.LogWarning("AudioSource or AudioClip not set in AudioPlayer.");
        }
    }
}
