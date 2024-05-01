using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterVolumeManager : MonoBehaviour
{
    public static MasterVolumeManager Instance { get; private set; }

    [Header("Master Settings")]
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField, Range(0f, 1f)] private float initialMasterVolume;

    private const string MASTER_VOLUME = "MasterVolume";

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeMasterVolume();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MasterVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeMasterVolume() => ChangeMasterVolume(initialMasterVolume);
    public void ChangeMasterVolume(float volume) => masterAudioMixer.SetFloat(MASTER_VOLUME, Mathf.Log10(volume) * 20);
}
