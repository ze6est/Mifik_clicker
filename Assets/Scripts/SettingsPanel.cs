using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;

    private void Awake()
    {
        _musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        _soundSlider.onValueChanged.AddListener(ChangeSoundVolume);        
    }

    private void OnDestroy()
    {
        _musicSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
        _soundSlider.onValueChanged.RemoveListener(ChangeSoundVolume);        
    }

    private void ChangeMusicVolume(float volume) => 
        _audioMixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, volume));

    private void ChangeSoundVolume(float volume) => 
        _audioMixer.audioMixer.SetFloat("SoundsVolume", Mathf.Lerp(-80, 0, volume));
}