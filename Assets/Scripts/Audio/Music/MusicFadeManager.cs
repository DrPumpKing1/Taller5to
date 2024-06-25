using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class MusicFadeManager : MonoBehaviour
{
    public static MusicFadeManager Instance { get; private set; }

    [Header("Volume Settings")]
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float muteTime;

    [Header("States")]
    [SerializeField] private State musicFadeState;

    public enum State {Muted,Idle,FadingIn,FadingOut}

    private const float MAX_VOLUME = 1f;
    private const float MIN_VOLUME = 0.0001f;
    private const float INITIAL_VOLUME = 1f;

    private const string MUSIC_FADE_VOLUME = "MusicFadeVolume";

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetMusicFadeState(State.Idle);
        InitializeVolume();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one MusicFadeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetMusicFadeState(State state) => musicFadeState = state;

    private void InitializeVolume() => ChangeVolume(INITIAL_VOLUME);

    private void ChangeVolume(float volume)
    {
        volume = volume < GetMinVolume() ? GetMinVolume() : volume;
        volume = volume > GetMaxVolume() ? GetMaxVolume() : volume;

        masterAudioMixer.SetFloat(MUSIC_FADE_VOLUME, Mathf.Log10(volume) * 20);
    }

    private float GetLogarithmicVolume()
    {
        masterAudioMixer.GetFloat(MUSIC_FADE_VOLUME, out float logarithmicVolume);
        return logarithmicVolume;
    }

    private float GetLinearVolume()
    {
        float logarithmicVolume = GetLogarithmicVolume();
        float linearVolume = Mathf.Pow(10f, logarithmicVolume / 20f);
        return linearVolume;
    }

    public float GetMaxVolume() => MAX_VOLUME;
    public float GetMinVolume() => MIN_VOLUME;

    #region Coroutines
    private IEnumerator FadeOutMusic(float fadeOutTime)
    {
        SetMusicFadeState(State.FadingOut);

        float initialVolume = GetLinearVolume();
        float realFadeOutTime = initialVolume * fadeOutTime;
        float time = 0;

        while (time < realFadeOutTime)
        {
            ChangeVolume(initialVolume * (1 - time / realFadeOutTime));
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        ChangeVolume(MIN_VOLUME);

        SetMusicFadeState(State.Muted);
    }

    private IEnumerator FadeInMusic(float fadeInTime)
    {
        SetMusicFadeState(State.FadingIn);

        float initialVolume = GetLinearVolume();
        float realFadeInTime = (1- initialVolume) * fadeInTime;
        float time = 0;

        while (time < realFadeInTime)
        {
            ChangeVolume(initialVolume * (1 + time / realFadeInTime));
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        ChangeVolume(MAX_VOLUME);

        SetMusicFadeState(State.Idle);
    }

    #endregion
}
