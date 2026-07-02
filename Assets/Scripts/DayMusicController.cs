using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DayMusicController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (DayNightCycle.Instance == null)
            return;

        if (DayNightCycle.Instance.IsNight())
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
        else
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}