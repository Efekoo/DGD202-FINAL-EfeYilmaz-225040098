using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource;

    [Header("UI Elements")]
    public TextMeshProUGUI soundButtonText;
    public Button soundToggleButton;
    public Slider volumeSlider;

    private bool isMuted;


    void Start()
    {
        isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        if (volumeSlider != null)
            volumeSlider.value = savedVolume;

        musicSource.volume = savedVolume;

        musicSource.mute = isMuted;

        if (volumeSlider != null)
            volumeSlider.value = savedVolume;

        UpdateSoundButton();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        musicSource.mute = isMuted;

        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateSoundButton();
    }

    void UpdateSoundButton()
    {
        if (soundButtonText != null)
        {
            soundButtonText.text = isMuted ? "Müzik: Kapalı" : "Müzik: Açık";
            soundButtonText.color = Color.white;
        }

        Image buttonImage = soundToggleButton?.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (isMuted)
            {
                buttonImage.enabled = true;
                buttonImage.color = Color.red;
            }
            else
            {
                buttonImage.enabled = false;
            }
        }
    }
    public void OnVolumeSliderChanged(float value)
    {
        musicSource.volume = value;

        if (value > 0f)
        {
            isMuted = false;
            musicSource.mute = false;
            PlayerPrefs.SetInt("MusicMuted", 0);
        }
        else
        {
            isMuted = true;
            musicSource.mute = true;
            PlayerPrefs.SetInt("MusicMuted", 1);
        }

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        UpdateSoundButton();
    }
}