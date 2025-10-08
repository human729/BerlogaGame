using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VolumeHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SoundSlider;
    [SerializeField] private float Multiplier;

    private void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicMixer");
        SoundSlider.value = PlayerPrefs.GetFloat("SoundMixer");
    }

    public void onMusicVolumeChange()
    {
        audioMixer.SetFloat("Music", MusicSlider.value);
        PlayerPrefs.SetFloat("MusicMixer", MusicSlider.value);
    }

    public void onSoundVolumeChange()
    {
        audioMixer.SetFloat("Sound", SoundSlider.value);
        PlayerPrefs.SetFloat("SoundMixer", SoundSlider.value);
    }
}
