using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SFXPoolSO SFXPoolSO;
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [Header("Settings")]
    [SerializeField, Range(0f,100f)] private float minDistance = 1f;
    [SerializeField, Range(0f, 1000)] private float maxDistance = 500f;
    [SerializeField, Range(0f,1)] private float spatialBlendFactor;
    [SerializeField] private AudioRolloffMode rollofMode;

    [Header("Debug")]
    [SerializeField] private bool debug;



    private void OnEnable()
    {
        ElectricalDoor.OnDoorPowered += ElectricalDoor_OnDoorPowered;
    }

    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e)
    {
        ElectricalDoor electricalDoor = sender as ElectricalDoor;
        PlaySound(SFXPoolSO.doorEnergized, electricalDoor.transform.position);
    }

    ///
    public void PlaySound(AudioClip[] audioClipArray, Vector3 position)
    {
        if(audioClipArray.Length == 0)
        {
            if (debug) Debug.Log("SFX play will be ignored, audioClipArray lenght is 0!");
            return;
        }

        AudioClip audioClip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        PlaySound(audioClip, position);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position)
    {
        GameObject sfxGameObject = new GameObject("TempSFX");
        sfxGameObject.transform.position = position;

        AudioSource tempAudioSource = sfxGameObject.AddComponent<AudioSource>();
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
