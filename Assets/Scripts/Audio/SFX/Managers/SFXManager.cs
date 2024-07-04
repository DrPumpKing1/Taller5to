using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SFXManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected SFXPoolSO SFXPoolSO;

    [Header("Settings")]
    [SerializeField] protected bool pauseOnPause;

    [Header("Temporal SFX AudioSource Settings")]
    [SerializeField] protected AudioMixerGroup audioMixerGroup;
    [SerializeField, Range(0f, 100f)] protected float minDistance = 1f;
    [SerializeField, Range(0f, 1000)] protected float maxDistance = 500f;
    [SerializeField, Range(0f, 1)] protected float spatialBlendFactor;
    [SerializeField] protected AudioRolloffMode rollofMode;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected AudioSource audioSource;
    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void PlaySound(AudioClip[] audioClipArray)
    {
        if (audioClipArray.Length == 0)
        {
            if (debug) Debug.Log("SFX play will be ignored, audioClipArray lenght is 0!");
            return;
        }

        AudioClip audioClip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        PlaySound(audioClip);
    }
    protected void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    protected void PlaySoundAtPoint(AudioClip[] audioClipArray, Vector3 position)
    {
        if (audioClipArray.Length == 0)
        {
            if (debug) Debug.Log("SFX play will be ignored, audioClipArray lenght is 0!");
            return;
        }

        AudioClip audioClip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        PlaySoundAtPoint(audioClip, position);
    }

    protected void PlaySoundAtPoint(AudioClip audioClip, Vector3 position)
    {
        GameObject sfxGameObject = new GameObject("TempSFX");
        sfxGameObject.transform.position = position;

        AudioSource tempAudioSource = sfxGameObject.AddComponent<AudioSource>();
        TemporalSFXController temporalSFXController = sfxGameObject.AddComponent<TemporalSFXController>();

        temporalSFXController.SetPausable(pauseOnPause);

        tempAudioSource.clip = audioClip;
        tempAudioSource.outputAudioMixerGroup = audioMixerGroup;

        tempAudioSource.spatialBlend = spatialBlendFactor; // Set spatial blending (0.0 for 2D, 1.0 for 3D)
        tempAudioSource.minDistance = minDistance; // Set the minimum distance for 3D sound
        tempAudioSource.maxDistance = maxDistance; // Set the maximum distance for 3D sound
        tempAudioSource.rolloffMode = rollofMode; // Set the rolloff mode

        tempAudioSource.Play();

        Destroy(sfxGameObject, audioClip.length);
    }

}
