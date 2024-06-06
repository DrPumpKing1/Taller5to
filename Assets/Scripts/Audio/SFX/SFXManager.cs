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
        ElectricalDoor.OnDoorDePowered += ElectricalDoor_OnDoorDePowered;
        ElectricalSwitchToggle.OnSwitchToggle += ElectricalSwitchToggle_OnSwitchToggle;

        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess += ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectionPlatformProjection.OnAnyObjectProjectionFailedInsuficientGems += ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems;

        ProjectableObjectDematerialization.OnAnyObjectDematerialized += ProjectableObjectDematerialization_OnAnyObjectDematerialized;

        ShieldPieceCollection.OnAnyShieldPieceCollected += ShieldPieceCollection_OnAnyShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
    }

    private void OnDisable()
    {
        ElectricalDoor.OnDoorPowered -= ElectricalDoor_OnDoorPowered;
        ElectricalDoor.OnDoorDePowered -= ElectricalDoor_OnDoorDePowered;
        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;

        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess -= ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectionPlatformProjection.OnAnyObjectProjectionFailedInsuficientGems -= ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems;

        ProjectableObjectDematerialization.OnAnyObjectDematerialized -= ProjectableObjectDematerialization_OnAnyObjectDematerialized;

        ShieldPieceCollection.OnAnyShieldPieceCollected -= ShieldPieceCollection_OnAnyShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
    }

    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e)
    {
        ElectricalSwitchToggle electricalSwitchToggle = sender as ElectricalSwitchToggle;
        PlaySound(SFXPoolSO.switchToggle, electricalSwitchToggle.transform.position);
    }

    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e)
    {
        ElectricalDoor electricalDoor = sender as ElectricalDoor;
        PlaySound(SFXPoolSO.doorEnergized, electricalDoor.transform.position);
    }

    private void ElectricalDoor_OnDoorDePowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e)
    {
        ElectricalDoor electricalDoor = sender as ElectricalDoor;
        PlaySound(SFXPoolSO.doorDeEnergized, electricalDoor.transform.position);
    }


    private void ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems(object sender, ProjectionPlatformProjection.OnAnyProjectionEventArgs e)
    {
        ProjectionPlatformProjection projectionPlatformProjection = sender as ProjectionPlatformProjection;
        PlaySound(SFXPoolSO.objectFailedProjection, projectionPlatformProjection.transform.position);
    }

    private void ProjectionPlatformProjection_OnAnyObjectProjectionSuccess(object sender, ProjectionPlatformProjection.OnAnyProjectionEventArgs e)
    {
        ProjectionPlatformProjection projectionPlatformProjection = sender as ProjectionPlatformProjection;
        PlaySound(SFXPoolSO.objectProjected, projectionPlatformProjection.transform.position);
    }

    private void ProjectableObjectDematerialization_OnAnyObjectDematerialized(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        ProjectableObjectDematerialization projectableObjectDematerialization = sender as ProjectableObjectDematerialization;
        PlaySound(SFXPoolSO.objectDematerialization, projectableObjectDematerialization.transform.position);
    }
    private void ShieldPieceCollection_OnAnyShieldPieceCollected(object sender, ShieldPieceCollection.OnAnyShieldPieceCollectedEventArgs e)
    {
        ShieldPieceCollection shieldPieceCollection = sender as ShieldPieceCollection;
        PlaySound(SFXPoolSO.shieldCollected, shieldPieceCollection.transform.position);
    }

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e)
    {
        ShieldDoor shieldDoor = sender as ShieldDoor;
        PlaySound(SFXPoolSO.valueDoorOpened, shieldDoor.transform.position);
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
        TemporalSFXController temporalSFXController = sfxGameObject.AddComponent<TemporalSFXController>();

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
