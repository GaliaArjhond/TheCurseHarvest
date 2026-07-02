using UnityEngine;

public class ToolSFX : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip axeSound;
    public AudioClip hoeSound;
    public AudioClip pickaxeSound;
    public AudioClip waterSound;

    public void PlayAxe()
    {
        audioSource.PlayOneShot(axeSound);
    }

    public void PlayHoe()
    {
        audioSource.PlayOneShot(hoeSound);
    }

    public void PlayPickaxe()
    {
        audioSource.PlayOneShot(pickaxeSound);
    }

    public void PlayWater()
    {
        audioSource.PlayOneShot(waterSound);
    }
}