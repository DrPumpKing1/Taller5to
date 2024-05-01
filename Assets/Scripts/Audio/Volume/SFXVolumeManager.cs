using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXVolumeManager : MonoBehaviour
{
    public static SFXVolumeManager Instance { get; private set; }

    [Header("SFX Settings")]
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField, Range(0f, 1f)] private float initialSFXVolume;

    private const string SFX_VOLUME = "SFXVolume";

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeSFXVolume();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one SFXVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeSFXVolume() => ChangeSFXVolume(initialSFXVolume);
    public void ChangeSFXVolume(float volume) => masterAudioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(volume) * 20);
}
