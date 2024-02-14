using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    [SerializeField] private Slider MusicVolumeControl;
    [SerializeField] private Slider SFXVolumeControl;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _buttonPopSound;
    [SerializeField] private AudioClip _lemonSelectedSound;

    public static AudioManager Instance { get; private set; }

    #region Unity Events

    private void Awake()
    {
        #region Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } 
        else
        {
            Instance = this;
        }
        #endregion


        MusicVolumeControl.onValueChanged.AddListener(OnMusicVolChanged);
        SFXVolumeControl.onValueChanged.AddListener(OnSFXVolChanged);

        MusicSource.volume = PlayerPrefs.GetFloat("musicvolume", 1f);
        SFXSource.volume = PlayerPrefs.GetFloat("sfxvolume", 1f);

        MusicVolumeControl.value = MusicSource.volume;
        SFXVolumeControl.value = SFXSource.volume;

    }

    #endregion

    #region Public Members

    public void PlaySFX(SFXSoundsEnum sound)
    {
        switch (sound)
        {
            case SFXSoundsEnum.ButtonPop:
                SFXSource.PlayOneShot(_buttonPopSound);
                break;
            case SFXSoundsEnum.LemonSpawns:
                break;
            case SFXSoundsEnum.LemonSelected:
                SFXSource.PlayOneShot(_lemonSelectedSound);
                break;
            default:
                break;
        }

        
    }

    #endregion



    #region Event Handlers
    private void OnMusicVolChanged(float arg0)
    {
        MusicSource.volume = arg0;

        PlayerPrefs.SetFloat("musicvolume", arg0);
    }

    private void OnSFXVolChanged(float arg0)
    {
        SFXSource.volume = arg0;

        PlayerPrefs.SetFloat("sfxvolume", arg0);
    }

    #endregion
}

public enum SFXSoundsEnum
{
    ButtonPop,
    LemonSelected,
    LemonSpawns
}
