 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PausableSFXManager : SFXManager
{
    private void OnEnable()
    {
        PlayerLand.OnPlayerNormalLand += PlayerLand_OnPlayerNormalLand;
        PlayerLand.OnPlayerHardLand += PlayerLand_OnPlayerHardLand;
        PetPlayerAttachment.OnVyrxInitialAttachToPlayer += PetPlayerAttachment_OnVyrxInitialAttachToPlayer;

        ElectricalSwitchToggle.OnSwitchToggle += ElectricalSwitchToggle_OnSwitchToggle;

        ElectricalDoor.OnDoorPower += ElectricalDoor_OnDoorPowered;
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

        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectsLearningManager_OnProjectableObjectLearned;

        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess += ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectionPlatformProjection.OnAnyObjectProjectionFailedInsuficientGems += ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems;
        ProjectableObjectDematerialization.OnAnyObjectDematerialized += ProjectableObjectDematerialization_OnAnyObjectDematerialized;
        ProjectionResetObject.OnAnyProjectionResetObjectUsed += ProjectionResetObject_OnAnyProjectionResetObjectUsed;

        ProjectableObjectRotation.OnAnyObjectRotated += ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated += ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated += ProjectableObjectActivation_OnAnyObjectDeactivated;

        DrainerDevice.OnDrainerStartDraining += DrainerDevice_OnDrainerStartDraining;
        DrainerDevice.OnDrainerStopDraining += DrainerDevice_OnDrainerStopDraining;

        SignalSender.OnAnyProjectileShot += SignalSender_OnProjectileShot;
        SignalProjectile.OnAnyProjectileImpact += SignalProjectile_OnAnyProjectileImpact;
    }
    private void OnDisable()
    {
        PlayerLand.OnPlayerNormalLand -= PlayerLand_OnPlayerNormalLand;
        PlayerLand.OnPlayerHardLand -= PlayerLand_OnPlayerHardLand;
        PetPlayerAttachment.OnVyrxInitialAttachToPlayer -= PetPlayerAttachment_OnVyrxInitialAttachToPlayer;

        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;

        ElectricalDoor.OnDoorPower -= ElectricalDoor_OnDoorPowered;
        ElectricalDoor.OnDoorDePowered -= ElectricalDoor_OnDoorDePowered;
        ElectricalDrawbridge.OnDrawbridgePower -= ElectricalDrawbridge_OnDrawbridgePower;
        ElectricalDrawbridge.OnDrawbridgeDePower -= ElectricalDrawbridge_OnDrawbridgeDePower;
        ElectricalExtensibleBridge.OnExtensibleBridgePower -= ElectricalExtensibleBridge_OnExtensibleBridgePower;
        ElectricalExtensibleBridge.OnExtensibleBridgeDePower -= ElectricalExtensibleBridge_OnExtensibleBridgeDePower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverPower -= HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverDePower -= HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower;

        InscriptionPowering.OnInscriptionPower -= InscriptionPowering_OnInscriptionPower;
        InscriptionPowering.OnInscriptionDePower -= InscriptionPowering_OnInscriptionDePower;

        ShieldPieceCollection.OnAnyShieldPieceCollected -= ShieldPieceCollection_OnAnyShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;

        ProjectionPlatformProjection.OnAnyObjectProjectionSuccess -= ProjectionPlatformProjection_OnAnyObjectProjectionSuccess;
        ProjectionPlatformProjection.OnAnyObjectProjectionFailedInsuficientGems -= ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems;
        ProjectableObjectDematerialization.OnAnyObjectDematerialized -= ProjectableObjectDematerialization_OnAnyObjectDematerialized;
        ProjectionResetObject.OnAnyProjectionResetObjectUsed -= ProjectionResetObject_OnAnyProjectionResetObjectUsed;

        ProjectableObjectRotation.OnAnyObjectRotated -= ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated -= ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated -= ProjectableObjectActivation_OnAnyObjectDeactivated;

        DrainerDevice.OnDrainerStartDraining -= DrainerDevice_OnDrainerStartDraining;
        DrainerDevice.OnDrainerStopDraining -= DrainerDevice_OnDrainerStopDraining;

        SignalSender.OnAnyProjectileShot -= SignalSender_OnProjectileShot;
        SignalProjectile.OnAnyProjectileImpact -= SignalProjectile_OnAnyProjectileImpact;
    } 

    #region Player
    private void PlayerLand_OnPlayerNormalLand(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.playerLand);
    }

    private void PlayerLand_OnPlayerHardLand(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.playerLand);
    }
    private void PetPlayerAttachment_OnVyrxInitialAttachToPlayer(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.vyrxAttach);
    }

    #endregion

    #region Electrical
    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e)
    {
        ElectricalSwitchToggle electricalSwitchToggle = sender as ElectricalSwitchToggle;
        PlaySoundAtPoint(SFXPoolSO.switchToggle, electricalSwitchToggle.transform.position);

        if (e.switchOn)
        {
            PlaySoundAtPoint(SFXPoolSO.switchOn, electricalSwitchToggle.transform.position);
        }
        else
        {
            PlaySoundAtPoint(SFXPoolSO.switchOff, electricalSwitchToggle.transform.position);
        }
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

    #region Learning
    private void ProjectableObjectsLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e)
    {
        PlaySound(SFXPoolSO.objectLearned);

        switch (e.projectableObjectLearned.id)
        {
            case 1:
                PlaySound(SFXPoolSO.cableLearned);
                break;
            case 2:
                PlaySound(SFXPoolSO.magicBoxLearned);
                break;
            case 3:
                PlaySound(SFXPoolSO.senderLearned);
                break;
            case 4:
                PlaySound(SFXPoolSO.drainerLearned);
                break;
        }
    }
    #endregion

    #region Projection
    private void ProjectionPlatformProjection_OnAnyObjectProjectionSuccess(object sender, ProjectionPlatformProjection.OnAnyProjectionEventArgs e)
    {
        ProjectionPlatformProjection projectionPlatformProjection = sender as ProjectionPlatformProjection;
        PlaySoundAtPoint(SFXPoolSO.objectProjected, projectionPlatformProjection.transform.position);

        switch (e.projectableObjectSO.id)
        {
            case 1:
                PlaySoundAtPoint(SFXPoolSO.cableProjected, projectionPlatformProjection.transform.position);
                break;
            case 2:
                PlaySoundAtPoint(SFXPoolSO.magicBoxProjected, projectionPlatformProjection.transform.position);
                break;
            case 3:
                PlaySoundAtPoint(SFXPoolSO.senderProjected, projectionPlatformProjection.transform.position);
                break;
            case 4:
                PlaySoundAtPoint(SFXPoolSO.drainerProjected, projectionPlatformProjection.transform.position);
                break;
        }
    }

    private void ProjectableObjectDematerialization_OnAnyObjectDematerialized(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        ProjectableObjectDematerialization projectableObjectDematerialization = sender as ProjectableObjectDematerialization;
        PlaySoundAtPoint(SFXPoolSO.objectDematerialization, projectableObjectDematerialization.transform.position);

        switch (e.projectableObjectSO.id)
        {
            case 1:
                PlaySoundAtPoint(SFXPoolSO.cableDematerialized, projectableObjectDematerialization.transform.position);
                break;
            case 2:
                PlaySoundAtPoint(SFXPoolSO.magicBoxDematerialized, projectableObjectDematerialization.transform.position);
                break;
            case 3:
                PlaySoundAtPoint(SFXPoolSO.senderDematerialized, projectableObjectDematerialization.transform.position);
                break;
            case 4:
                PlaySoundAtPoint(SFXPoolSO.drainerDematerialized, projectableObjectDematerialization.transform.position);
                break;
        }
    }

    private void ProjectionResetObject_OnAnyProjectionResetObjectUsed(object sender, System.EventArgs e)
    {
        ProjectionResetObject projectionResetObject = sender as ProjectionResetObject;
        PlaySoundAtPoint(SFXPoolSO.projectionResetObjectUsed, projectionResetObject.transform.position);
    }

    private void ProjectionPlatformProjection_OnAnyObjectProjectionFailedInsuficientGems(object sender, ProjectionPlatformProjection.OnAnyProjectionEventArgs e)
    {
        ProjectionPlatformProjection projectionPlatformProjection = sender as ProjectionPlatformProjection;
        PlaySoundAtPoint(SFXPoolSO.objectFailedProjection, projectionPlatformProjection.transform.position);
    }
    #endregion

    #region Projection Alternate
    private void ProjectableObjectRotation_OnAnyObjectRotated(object sender, ProjectableObjectRotation.OnAnyObjectRotatedEventArgs e)
    {
        ProjectableObjectRotation projectableObjectRotation = sender as ProjectableObjectRotation;
        PlaySoundAtPoint(SFXPoolSO.objectRotated, projectableObjectRotation.transform.position);

        switch (e.projectableObjectSO.id)
        {
            case 1:
                PlaySoundAtPoint(SFXPoolSO.cableRotated, projectableObjectRotation.transform.position);
                break;
            case 3:
                PlaySoundAtPoint(SFXPoolSO.senderRotated, projectableObjectRotation.transform.position);
                break;
        }
    }
    private void ProjectableObjectActivation_OnAnyObjectActivated(object sender, ProjectableObjectActivation.OnAnyObjectActivatedEventArgs e)
    {
        ProjectableObjectActivation projectableObjectActivation = sender as ProjectableObjectActivation;
        PlaySoundAtPoint(SFXPoolSO.objectActivation, projectableObjectActivation.transform.position);

        switch (e.projectableObjectSO.id)
        {
            case 2:
                PlaySoundAtPoint(SFXPoolSO.magicBoxActivated, projectableObjectActivation.transform.position);
                break;
        }
    }

    private void ProjectableObjectActivation_OnAnyObjectDeactivated(object sender, ProjectableObjectActivation.OnAnyObjectActivatedEventArgs e)
    {
        ProjectableObjectActivation projectableObjectActivation = sender as ProjectableObjectActivation;
        PlaySoundAtPoint(SFXPoolSO.objectDeactivation, projectableObjectActivation.transform.position);

        switch (e.projectableObjectSO.id)
        {
            case 2:
                PlaySoundAtPoint(SFXPoolSO.magicBoxDeactivated, projectableObjectActivation.transform.position);
                break;
        }
    }

    private void DrainerDevice_OnDrainerStartDraining(object sender, System.EventArgs e)
    {
        DrainerDevice drainerDevice = sender as DrainerDevice;
        PlaySoundAtPoint(SFXPoolSO.drainerActivated, drainerDevice.transform.position);
    }
    private void DrainerDevice_OnDrainerStopDraining(object sender, System.EventArgs e)
    {
        DrainerDevice drainerDevice = sender as DrainerDevice;
        PlaySoundAtPoint(SFXPoolSO.drainerDeactivated, drainerDevice.transform.position);
    }

    #endregion

    #region Shields
    private void ShieldPieceCollection_OnAnyShieldPieceCollected(object sender, ShieldPieceCollection.OnAnyShieldPieceCollectedEventArgs e)
    {
        ShieldPieceCollection shieldPieceCollection = sender as ShieldPieceCollection;
        PlaySoundAtPoint(SFXPoolSO.shieldCollected, shieldPieceCollection.transform.position);

        switch (e.shieldPieceSO.dialect)
        {
            case Dialect.Zurryth:
                PlaySoundAtPoint(SFXPoolSO.zurrythShieldCollected, shieldPieceCollection.transform.position);
                break;
            case Dialect.Rakithu:
                PlaySoundAtPoint(SFXPoolSO.rakithuShieldCollected, shieldPieceCollection.transform.position);
                break;
            case Dialect.Xotark:
                PlaySoundAtPoint(SFXPoolSO.xotarkShieldCollected, shieldPieceCollection.transform.position);
                break;
            case Dialect.Vythanu:
                PlaySoundAtPoint(SFXPoolSO.vythanuShieldCollected, shieldPieceCollection.transform.position);
                break;
        }
    }

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e)
    {
        ShieldDoor shieldDoor = sender as ShieldDoor;
        PlaySoundAtPoint(SFXPoolSO.valueDoorOpened, shieldDoor.transform.position);

        switch (e.dialect)
        {
            case Dialect.Zurryth:
                PlaySoundAtPoint(SFXPoolSO.zurrythValueDoorOpened, shieldDoor.transform.position);
                break;
            case Dialect.Rakithu:
                PlaySoundAtPoint(SFXPoolSO.rakithuValueDoorOpened, shieldDoor.transform.position);
                break;
            case Dialect.Xotark:
                PlaySoundAtPoint(SFXPoolSO.xotarkValueDoorOpened, shieldDoor.transform.position);
                break;
            case Dialect.Vythanu:
                PlaySoundAtPoint(SFXPoolSO.vythanuValueDoorOpened, shieldDoor.transform.position);
                break;
        }
    }
    #endregion

    #region Projectiles
    private void SignalSender_OnProjectileShot(object sender, System.EventArgs e)
    {
        SignalSender signalSender = sender as SignalSender;
        PlaySoundAtPoint(SFXPoolSO.projectileShot, signalSender.transform.position);
    }
    private void SignalProjectile_OnAnyProjectileImpact(object sender, System.EventArgs e)
    {
        SignalProjectile singalProjectile = sender as SignalProjectile;
        PlaySoundAtPoint(SFXPoolSO.projectileImpact, singalProjectile.transform.position);
    }

    #endregion
}
