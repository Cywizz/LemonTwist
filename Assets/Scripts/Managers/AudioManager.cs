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
    [SerializeField] private AudioClip _lemonSpawnSound;
    [SerializeField] private AudioClip _keyPickupSound;
    [SerializeField] private AudioClip _doorOpenSound;
    [SerializeField] private AudioClip _juicingSound;
    [SerializeField] private AudioClip _diggingSound;
    [SerializeField] private AudioClip _buildingSound;
    [SerializeField] private AudioClip _lemonEntersDoor;
    [SerializeField] private AudioClip _rewardItemSound;
    [SerializeField] private AudioClip _bashingSound;


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

        MusicSource.volume = PlayerPrefs.GetFloat("musicvolume", 0.8f);
        SFXSource.volume = PlayerPrefs.GetFloat("sfxvolume", 0.6f);

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
            case SFXSoundsEnum.LemonSelected:
                SFXSource.PlayOneShot(_lemonSelectedSound);
                break;
            case SFXSoundsEnum.LemonSpawns:
                SFXSource.PlayOneShot(_lemonSpawnSound);
                break;
            case SFXSoundsEnum.KeyPickup:
                SFXSource.PlayOneShot(_keyPickupSound);
                break;
            case SFXSoundsEnum.DoorOpen:
                SFXSource.PlayOneShot(_doorOpenSound);
                break;
            case SFXSoundsEnum.Juicing:
                SFXSource.PlayOneShot(_juicingSound);
                break;
            case SFXSoundsEnum.Digging:
                SFXSource.PlayOneShot(_diggingSound);
                break;
            case SFXSoundsEnum.Building:
                SFXSource.PlayOneShot(_buildingSound);
                break;
            case SFXSoundsEnum.EnterDoor:
                SFXSource.PlayOneShot(_lemonEntersDoor);
                break;
            case SFXSoundsEnum.RewardItem:
                SFXSource.PlayOneShot(_rewardItemSound);
                break;
            case SFXSoundsEnum.Bashing:
                SFXSource.PlayOneShot(_bashingSound);
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
    LemonSpawns,
    KeyPickup,
    DoorOpen,
    Juicing,
    Digging,
    Building,
    EnterDoor,
    RewardItem,
    Bashing
}
