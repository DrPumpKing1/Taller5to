using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SFXPoolSO SFXPoolSO;

    [Header("Temporal SFX AudioSource Settings")]
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField, Range(0f,100f)] private float minDistance = 1f;
    [SerializeField, Range(0f, 1000)] private float maxDistance = 500f;
    [SerializeField, Range(0f,1)] private float spatialBlendFactor;
    [SerializeField] private AudioRolloffMode rollofMode;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private AudioSource audioSource;

    private void OnEnable()
    {
        ElectricalSwitchToggle.OnSwitchToggle += ElectricalSwitchToggle_OnSwitchToggle;

        ElectricalDoor.OnDoorPowe += ElectricalDoor_OnDoorPowered;
        ElectricalDoor.OnDoorDePowered += ElectricalDoor_OnDoorDePowered;
        ElectricalDrawbridge.OnDrawbridgePower += ElectricalDrawbridge_OnDrawbridgePower;
        ElectricalDrawbridge.OnDrawbridgeDePower += ElectricalDrawbridge_OnDrawbridgeDePower;
        ElectricalExtensibleBridge.OnExtensibleBridgePower += ElectricalExtensibleBridge_OnExtensibleBridgePower;
        ElectricalExtensibleBridge.OnExtensibleBridgeDePower += ElectricalExtensibleBridge_OnExtensibleBridgeDePower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverPower += HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverDePower += HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower;

        InscriptionPowering.OnInscriptionPower += InscriptionPowering_OnInscriptionPower;
        InscriptionPowering.OnInscriptionDePower += InscriptionPowering_OnInscriptionDePower;

        ShieldPieceCollection.OnAnyShieldPieceCollected += ShieldPieceCollection_OnAnyShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;

        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess += ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectionPlatformProjection.OnAnyObjectProjectionFailedInsuficientGems += ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems;
        ProjectableObjectDematerialization.OnAnyObjectDematerialized += ProjectableObjectDematerialization_OnAnyObjectDematerialized;
        ProjectionResetObject.OnAnyProjectionResetObjectUsed += ProjectionResetObject_OnAnyProjectionResetObjectUsed;

        ProjectableObjectRotation.OnAnyObjectRotated += ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated += ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated += ProjectableObjectActivation_OnAnyObjectDeativated;
    }

    private void OnDisable()
    {
        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;

        ElectricalDoor.OnDoorPowe -= ElectricalDoor_OnDoorPowered;
        ElectricalDoor.OnDoorDePowered -= ElectricalDoor_OnDoorDePowered;
        ElectricalDrawbridge.OnDrawbridgePower -= ElectricalDrawbridge_OnDrawbridgePower;
        ElectricalDrawbridge.OnDrawbridgeDePower -= ElectricalDrawbridge_OnDrawbridgeDePower;
        ElectricalExtensibleBridge.OnExtensibleBridgePower -= ElectricalExtensibleBridge_OnExtensibleBridgePower;
        ElectricalExtensibleBridge.OnExtensibleBridgeDePower -= ElectricalExtensibleBridge_OnExtensibleBridgeDePower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverPower -= HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverDePower -= HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower;

        ShieldPieceCollection.OnAnyShieldPieceCollected -= ShieldPieceCollection_OnAnyShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;

        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess -= ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectionPlatformProjection.OnAnyObjectProjectionFailedInsuficientGems -= ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems;
        ProjectableObjectDematerialization.OnAnyObjectDematerialized -= ProjectableObjectDematerialization_OnAnyObjectDematerialized;
        ProjectionResetObject.OnAnyProjectionResetObjectUsed -= ProjectionResetObject_OnAnyProjectionResetObjectUsed;

        ProjectableObjectRotation.OnAnyObjectRotated -= ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated -= ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated -= ProjectableObjectActivation_OnAnyObjectDeativated;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    #region Electrical
    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e)
    {
        ElectricalSwitchToggle electricalSwitchToggle = sender as ElectricalSwitchToggle;
        PlaySoundAtPoint(SFXPoolSO.switchToggle, electricalSwitchToggle.transform.position);
    }

    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e)
    {
        ElectricalDoor electricalDoor = sender as ElectricalDoor;
        PlaySoundAtPoint(SFXPoolSO.doorPowered, electricalDoor.transform.position);
    }

    private void ElectricalDoor_OnDoorDePowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e)
    {
        ElectricalDoor electricalDoor = sender as ElectricalDoor;
        PlaySoundAtPoint(SFXPoolSO.doorDePowered, electricalDoor.transform.position);
    }

    private void ElectricalDrawbridge_OnDrawbridgePower(object sender, ElectricalDrawbridge.OnDrawbridgePoweredEventArgs e)
    {
        ElectricalDrawbridge electricalDrawbridge = sender as ElectricalDrawbridge;
        PlaySoundAtPoint(SFXPoolSO.drawbridgePowered, electricalDrawbridge.transform.position);
    }

    private void ElectricalDrawbridge_OnDrawbridgeDePower(object sender, ElectricalDrawbridge.OnDrawbridgePoweredEventArgs e)
    {
        ElectricalDrawbridge electricalDrawbridge = sender as ElectricalDrawbridge;
        PlaySoundAtPoint(SFXPoolSO.drawbridgeDePowered, electricalDrawbridge.transform.position);
    }

    private void ElectricalExtensibleBridge_OnExtensibleBridgePower(object sender, ElectricalExtensibleBridge.OnExtensibleBridgePowerEventArgs e)
    {
        ElectricalExtensibleBridge extensibleBridge = sender as ElectricalExtensibleBridge;
        PlaySoundAtPoint(SFXPoolSO.extensibleBridgePowered, extensibleBridge.transform.position);
    }

    private void ElectricalExtensibleBridge_OnExtensibleBridgeDePower(object sender, ElectricalExtensibleBridge.OnExtensibleBridgePowerEventArgs e)
    {
        ElectricalExtensibleBridge extensibleBridge = sender as ElectricalExtensibleBridge;
        PlaySoundAtPoint(SFXPoolSO.extensibleBridgeDePowered, extensibleBridge.transform.position);
    }

    private void InscriptionPowering_OnInscriptionPower(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e)
    {
        InscriptionPowering inscriptionPowering = sender as InscriptionPowering;
        PlaySoundAtPoint(SFXPoolSO.inscriptionPowered, inscriptionPowering.transform.position);
    }
    private void InscriptionPowering_OnInscriptionDePower(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e)
    {
        InscriptionPowering inscriptionPowering = sender as InscriptionPowering;
        PlaySoundAtPoint(SFXPoolSO.inscriptionDePowered, inscriptionPowering.transform.position);
    }
    private void HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower(object sender, HiddenSourceReceiver.OnHiddenSourcePowerEventArgs e)
    {
        HiddenSourceReceiver hiddenSourceReceiver = sender as HiddenSourceReceiver;
        PlaySoundAtPoint(SFXPoolSO.receiverPowered, hiddenSourceReceiver.transform.position);
    }

    private void HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower(object sender, HiddenSourceReceiver.OnHiddenSourcePowerEventArgs e)
    {
        HiddenSourceReceiver hiddenSourceReceiver = sender as HiddenSourceReceiver;
        PlaySoundAtPoint(SFXPoolSO.receiverDePowered, hiddenSourceReceiver.transform.position);
    }

    #endregion

    #region Projection
    private void ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems(object sender, ProjectionPlatformProjection.OnAnyProjectionEventArgs e)
    {
        ProjectionPlatformProjection projectionPlatformProjection = sender as ProjectionPlatformProjection;
        PlaySoundAtPoint(SFXPoolSO.objectFailedProjection, projectionPlatformProjection.transform.position);
    }

    private void ProjectionPlatformProjection_OnAnyObjectProjectionSuccess(object sender, ProjectionPlatformProjection.OnAnyProjectionEventArgs e)
    {
        ProjectionPlatformProjection projectionPlatformProjection = sender as ProjectionPlatformProjection;
        PlaySoundAtPoint(SFXPoolSO.objectProjected, projectionPlatformProjection.transform.position);
    }

    private void ProjectableObjectDematerialization_OnAnyObjectDematerialized(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        ProjectableObjectDematerialization projectableObjectDematerialization = sender as ProjectableObjectDematerialization;
        PlaySoundAtPoint(SFXPoolSO.objectDematerialization, projectableObjectDematerialization.transform.position);
    }

    private void ProjectionResetObject_OnAnyProjectionResetObjectUsed(object sender, System.EventArgs e)
    {
        ProjectionResetObject projectionResetObject = sender as ProjectionResetObject;
        PlaySoundAtPoint(SFXPoolSO.projectionResetObjectUsed, projectionResetObject.transform.position);
    }
    #endregion

    #region Projection Alternate
    private void ProjectableObjectRotation_OnAnyObjectRotated(object sender, ProjectableObjectRotation.OnAnyObjectRotatedEventArgs e)
    {
        ProjectableObjectRotation projectableObjectRotation = sender as ProjectableObjectRotation;
        PlaySoundAtPoint(SFXPoolSO.objectRotated, projectableObjectRotation.transform.position);
    }
    private void ProjectableObjectActivation_OnAnyObjectDeativated(object sender, ProjectableObjectActivation.OnAnyObjectActivatedEventArgs e)
    {
        ProjectableObjectActivation projectableObjectActivation = sender as ProjectableObjectActivation;
        PlaySoundAtPoint(SFXPoolSO.objectActivation, projectableObjectActivation.transform.position);
    }

    private void ProjectableObjectActivation_OnAnyObjectActivated(object sender, ProjectableObjectActivation.OnAnyObjectActivatedEventArgs e)
    {
        ProjectableObjectActivation projectableObjectActivation = sender as ProjectableObjectActivation;
        PlaySoundAtPoint(SFXPoolSO.objectDeactivation, projectableObjectActivation.transform.position);
    }
    #endregion

    #region Shields
    private void ShieldPieceCollection_OnAnyShieldPieceCollected(object sender, ShieldPieceCollection.OnAnyShieldPieceCollectedEventArgs e)
    {
        ShieldPieceCollection shieldPieceCollection = sender as ShieldPieceCollection;
        PlaySoundAtPoint(SFXPoolSO.shieldCollected, shieldPieceCollection.transform.position);
    }

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e)
    {
        ShieldDoor shieldDoor = sender as ShieldDoor;
        PlaySoundAtPoint(SFXPoolSO.valueDoorOpened, shieldDoor.transform.position);
    }
    #endregion

    ///
    private void PlaySound(AudioClip[] audioClipArray)
    {
        if (audioClipArray.Length == 0)
        {
            if (debug) Debug.Log("SFX play will be ignored, audioClipArray lenght is 0!");
            return;
        }

        AudioClip audioClip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        PlaySound(audioClip);
    }
    private void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void PlaySoundAtPoint(AudioClip[] audioClipArray, Vector3 position)
    {
        if(audioClipArray.Length == 0)
        {
            if (debug) Debug.Log("SFX play will be ignored, audioClipArray lenght is 0!");
            return;
        }

        AudioClip audioClip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        PlaySoundAtPoint(audioClip, position);
    }
    private void PlaySoundAtPoint(AudioClip audioClip, Vector3 position)
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
