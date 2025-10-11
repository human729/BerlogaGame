using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SoundSlider;
    [SerializeField] private float Multiplier = 30f;

    private void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicMixer", 0.75f);
        SoundSlider.value = PlayerPrefs.GetFloat("SoundMixer", 0.75f);
        SetMusicVolume(MusicSlider.value);
        SetSoundVolume(SoundSlider.value);
    }

    public void onMusicVolumeChange()
    {
        SetMusicVolume(MusicSlider.value);
        PlayerPrefs.SetFloat("MusicMixer", MusicSlider.value);
    }

    public void onSoundVolumeChange()
    {
        SetSoundVolume(SoundSlider.value);
        PlayerPrefs.SetFloat("SoundMixer", SoundSlider.value);
    }

    private void SetMusicVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        audioMixer.SetFloat("Music", volume);
    }

    private void SetSoundVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        audioMixer.SetFloat("Sound", volume);
    }
}