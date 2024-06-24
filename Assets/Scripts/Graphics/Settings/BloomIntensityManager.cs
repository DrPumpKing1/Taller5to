using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

public class BloomIntensityManager : MonoBehaviour
{
    public static BloomIntensityManager Instance { get; private set; }

    [Header("Intensity Settings")]
    [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField, Range(0f, 1f)] protected float initialNormalizedIntensity;

    [Header("Load Settings")]
    [SerializeField] private string playerPrefsKey;

    private Bloom bloom;

    public static event EventHandler OnBloomIntensityManagerInitialized;
    public static event EventHandler<OnIntensityChangedEventArgs> OnBloomIntensityChanged;

    private const float MAX_NORMALIZED_INTENSITY = 1f;
    private const float MIN_NORMALIZED_INTENSITY = 0f;

    private const float MAX_INTENSITY = 1.5f;
    private const float MIN_INTENSITY = 0f;

    private const float DEFAULT_NORMALIZED_INTENSITY = 0.5f;

    public class OnIntensityChangedEventArgs : EventArgs
    {
        public float newIntensity;
    }
    private void Awake()
    {
        SetSingleton();
        InitializeBloom();
    }

    private void Start()
    {
        LoadIntensityPlayerPrefs();
        InitializeIntensity();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one BloomIntensityManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeBloom()
    {
        if (!volumeProfile.TryGet(out bloom))
        {
            Debug.LogError("Bloom settings not found in the Post Process Volume");
        }
    }

    private void LoadIntensityPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey(playerPrefsKey))
        {
            PlayerPrefs.SetFloat(playerPrefsKey, DEFAULT_NORMALIZED_INTENSITY);
        }

        initialNormalizedIntensity = PlayerPrefs.GetFloat(playerPrefsKey);
    }

    private void SaveIntensityPlayerPrefs(float intensity)
    {
        PlayerPrefs.SetFloat(playerPrefsKey, intensity);
    }

    private void InitializeIntensity()
    {
        ChangeIntensity(initialNormalizedIntensity);
        OnBloomIntensityManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeIntensity(float normalizedIntensity)
    {
        normalizedIntensity = normalizedIntensity < GetMinNormalizedIntensity() ? GetMinNormalizedIntensity() : normalizedIntensity;
        normalizedIntensity = normalizedIntensity > GetMaxNormalizedIntensity() ? GetMaxNormalizedIntensity() : normalizedIntensity;

        if (bloom) SetBloomNormalizedIntensity(normalizedIntensity);

        SaveIntensityPlayerPrefs(normalizedIntensity);

        OnBloomIntensityChanged?.Invoke(this, new OnIntensityChangedEventArgs { newIntensity = normalizedIntensity });
    }

    private void SetBloomNormalizedIntensity(float normalizedIntensity) => SetBloomIntensity(normalizedIntensity*GetMaxIntensity());

    private void SetBloomIntensity(float intensity) => bloom.intensity.value = intensity;

    public float GetNormalizedIntensity() => GetIntensity()/GetMaxIntensity();

    private float GetIntensity() => bloom.intensity.value;


    public float GetMaxNormalizedIntensity() => MAX_NORMALIZED_INTENSITY;
    public float GetMinNormalizedIntensity() => MIN_NORMALIZED_INTENSITY;
    public float GetMaxIntensity() => MAX_INTENSITY;
    public float GetMinIntensity() => MIN_INTENSITY;
}
