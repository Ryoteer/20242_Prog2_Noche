using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("<color=orange>Audio</color>")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _source;

    private float _masterVol = 0.0f;
    public float MasterVolume 
    { 
        get { return _masterVol; }
        set { _masterVol = value; }
    }

    private float _musicVol = 0.0f;
    public float MusicVolume
    {
        get { return _musicVol; }
        set { _musicVol = value; }
    }

    private float _sfxVol = 0.0f;
    public float SfxVolume
    {
        get { return _sfxVol; }
        set { _sfxVol = value; }
    }

    public void SetMasterVolume(float value)
    {
        _masterVol = value;
        _mixer.SetFloat("Master", Mathf.Log10(value) * 20.0f);
    }

    public void SetMusicVolume(float value)
    {
        _musicVol = value;
        _mixer.SetFloat("Music", Mathf.Log10(value) * 20.0f);
    }

    public void SetSfxVolume(float value)
    {
        _sfxVol = value;
        _mixer.SetFloat("SFX", Mathf.Log10(value) * 20.0f);
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip == _source.clip) return;

        _source.Stop();

        _source.clip = clip;

        _source.Play();
    }
}
