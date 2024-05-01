using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicVolumeManager : MonoBehaviour
{
    public static MusicVolumeManager Instance { get; private set; }

    [Header("Music Settings")]
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField, Range(0f, 1f)] private float initialMusicVolume;

    private const string MUSIC_VOLUME = "MusicVolume";

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeMusicVolume();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MusicVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeMusicVolume() => ChangeMusicVolume(initialMusicVolume);
    public void ChangeMusicVolume(float volume) => masterAudioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(volume) * 20);
}
