using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("ButtonSound: AudioSource eksik!");
            enabled = false;
        }
    }

    public void PlayClick()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound, 0.5f);
        }
    }
}