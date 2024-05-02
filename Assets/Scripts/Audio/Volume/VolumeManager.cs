using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public abstract class VolumeManager : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] protected AudioMixer masterAudioMixer;
    [SerializeField, Range(0f, 1f)] protected float initialVolume;

    private const float MAX_VOLUME = 1f;
    private const float MIN_VOLUME = 0.0001f;

    public class OnVolumeChangedEventArgs: EventArgs
    {
        public float newVolume;
    }

    private void Start()
    {
        InitializeVolume();
    }

    protected virtual void InitializeVolume() => ChangeVolume(initialVolume);
    public abstract void ChangeVolume(float volume);
    public abstract float GetLogarithmicVolume();
    public abstract float GetLinearVolume();
    public float GetMaxVolume() => MAX_VOLUME;
    public float GetMinVolume() => MIN_VOLUME;
}
