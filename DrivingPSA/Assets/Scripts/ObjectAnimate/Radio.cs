using System.Linq;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] audioClips;

    private bool isPlaying;

    public void ToggleInteraction()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
