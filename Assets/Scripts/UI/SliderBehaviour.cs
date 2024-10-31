using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour
{
    [Header("<color=orange>Audio</color>")]
    [Range(0.0f, 1.0f)][SerializeField] private float _masterVolume = 1.0f;
    [Range(0.0f, 1.0f)][SerializeField] private float _musicVolume = 0.5f;
    [Range(0.0f, 1.0f)][SerializeField] private float _sfxVolume = 0.875f;

    [Header("<color=yellow>UI</color>")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        if(AudioManager.Instance.MasterVolume == 0.0f)
        {
            _masterSlider.value = _masterVolume;
            AudioManager.Instance.SetMasterVolume(_masterVolume);
        }
        else
        {
            _masterSlider.value = AudioManager.Instance.MasterVolume;
        }

        if (AudioManager.Instance.MusicVolume == 0.0f)
        {
            _musicSlider.value = _musicVolume;
            AudioManager.Instance.SetMusicVolume(_musicVolume);
        }
        else
        {
            _musicSlider.value = AudioManager.Instance.MusicVolume;
        }

        if (AudioManager.Instance.SfxVolume == 0.0f)
        {
            _sfxSlider.value = _sfxVolume;
            AudioManager.Instance.SetSfxVolume(_sfxVolume);
        }
        else
        {
            _sfxSlider.value = AudioManager.Instance.SfxVolume;
        }
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }
}
