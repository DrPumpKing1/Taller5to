using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class VolumeManager : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] protected AudioMixer masterAudioMixer;
    [SerializeField, Range(0f, 1f)] protected float initialVolume;

    private const float MAX_VOLUME = 1F;
    private const float MIN_VOLUME = 0.0001f;

    private void Start()
    {
        InitializeVolume();
    }

    protected virtual void InitializeVolume() => ChangeVolume(initialVolume);
    public abstract void ChangeVolume(float volume);
    public abstract float GetVolume();
    public float GetMaxVolume() => MAX_VOLUME;
    public float GetMinVolume() => MIN_VOLUME;
}
