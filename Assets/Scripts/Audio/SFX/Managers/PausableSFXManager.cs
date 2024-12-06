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
        ElectricalExtensibleBridgeOld.OnExtensibleBridgePower += ElectricalExtensibleBridge_OnExtensibleBridgePower;
        ElectricalExtensibleBridgeOld.OnExtensibleBridgeDePower += ElectricalExtensibleBridge_OnExtensibleBridgeDePower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverPower += HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverDePower += HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower;

        InscriptionPowering.OnInscriptionPower += InscriptionPowering_OnInscriptionPower;
        InscriptionPowering.OnInscriptionDePower += InscriptionPowering_OnInscriptionDePower;

        ShieldPieceCollection.OnAnyShieldPieceCollected += ShieldPieceCollection_OnAnyShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
        ShieldsPopUpUI.OnShieldPopUpShow += ShieldsPopUpUI_OnShieldPopUpShow;
        ShieldsPopUpUI.OnShieldPopUpComplete += ShieldsPopUpUI_OnShieldPopUpComplete;
        ShieldsPopUpUI.OnShieldPopUpHide += ShieldsPopUpUI_OnShieldPopUpHide;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectsLearningManager_OnProjectableObjectLearned;

        ProjectableObjectSelectionManager.OnProjectableObjectSelected += ProjectableObjectSelectionManager_OnProjectableObjectSelected;

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

        BossBeam.OnBeamChargeStart += BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd += BossBeam_OnBeamChargeEnd;
        BossBeam.OnBeamPlatformTargeted += BossBeam_OnBeamPlatformTargeted;
        BossBeam.OnBeamPlatformStun += BossBeam_OnBeamPlatformStun;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossShield.OnAnyBossShieldActivated += BossShield_OnAnyBossShieldActivated;
        BossShield.OnAnyBossShieldDeactivated += BossShield_OnBossShieldDeactivated;
        BossStateHandler.OnBossAlmostDefeated += BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;

        ShowcaseRoomBeam.OnBeamChargeStart += ShowcaseRoomBeam_OnBeamChargeStart;
        ShowcaseRoomBeam.OnBeamChargeEnd += ShowcaseRoomBeam_OnBeamChargeEnd;
        ShowcaseRoomBeam.OnBeamPlatformTargeted += ShowcaseRoomBeam_OnBeamPlatformTargeted;
        ShowcaseRoomBeam.OnBeamPlatformStun += ShowcaseRoomBeam_OnBeamPlatformStun;
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeStart += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeStart;
        ShowcaseRoomShield.OnAnyShowcaseRoomShieldActivated += ShowcaseRoomShield_OnAnyShowcaseRoomShieldActivated;
        ShowcaseRoomShield.OnAnyShowcaseRoomShieldDeactivated += ShowcaseRoomShield_OnAnyShowcaseRoomShieldDeactivated;

        AncientRelicShield.OnAncientRelicShieldDepowered += AncientRelicShield_OnAncientRelicShieldDepowered;
        AncientRelic.OnAncientRelicCollected += AncientRelic_OnAncientRelicCollected;

        DialogueManager.OnDialogueStart += DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd += DialogueManager_OnDialogueEnd;
        MonologueManager.OnMonologueStart += MonologueManager_OnMonologueStart;
        MonologueManager.OnMonologueEnd += MonologueManager_OnMonologueEnd;
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
        ElectricalExtensibleBridgeOld.OnExtensibleBridgePower -= ElectricalExtensibleBridge_OnExtensibleBridgePower;
        ElectricalExtensibleBridgeOld.OnExtensibleBridgeDePower -= ElectricalExtensibleBridge_OnExtensibleBridgeDePower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverPower -= HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverDePower -= HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower;

        InscriptionPowering.OnInscriptionPower -= InscriptionPowering_OnInscriptionPower;
        InscriptionPowering.OnInscriptionDePower -= InscriptionPowering_OnInscriptionDePower;

        ShieldPieceCollection.OnAnyShieldPieceCollected -= ShieldPieceCollection_OnAnyShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
        ShieldsPopUpUI.OnShieldPopUpShow -= ShieldsPopUpUI_OnShieldPopUpShow;
        ShieldsPopUpUI.OnShieldPopUpComplete -= ShieldsPopUpUI_OnShieldPopUpComplete;
        ShieldsPopUpUI.OnShieldPopUpHide -= ShieldsPopUpUI_OnShieldPopUpHide;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;

        ProjectableObjectSelectionManager.OnProjectableObjectSelected -= ProjectableObjectSelectionManager_OnProjectableObjectSelected;

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

        BossBeam.OnBeamChargeStart -= BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd -= BossBeam_OnBeamChargeEnd;
        BossBeam.OnBeamPlatformTargeted -= BossBeam_OnBeamPlatformTargeted;
        BossBeam.OnBeamPlatformStun -= BossBeam_OnBeamPlatformStun;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossShield.OnAnyBossShieldDeactivated -= BossShield_OnBossShieldDeactivated;
        BossStateHandler.OnBossAlmostDefeated -= BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;

        ShowcaseRoomBeam.OnBeamChargeStart -= ShowcaseRoomBeam_OnBeamChargeStart;
        ShowcaseRoomBeam.OnBeamChargeEnd -= ShowcaseRoomBeam_OnBeamChargeEnd;
        ShowcaseRoomBeam.OnBeamPlatformTargeted -= ShowcaseRoomBeam_OnBeamPlatformTargeted;
        ShowcaseRoomBeam.OnBeamPlatformStun -= ShowcaseRoomBeam_OnBeamPlatformStun;
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeStart -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeStart;
        ShowcaseRoomShield.OnAnyShowcaseRoomShieldActivated -= ShowcaseRoomShield_OnAnyShowcaseRoomShieldActivated;
        ShowcaseRoomShield.OnAnyShowcaseRoomShieldDeactivated -= ShowcaseRoomShield_OnAnyShowcaseRoomShieldDeactivated;

        AncientRelicShield.OnAncientRelicShieldDepowered -= AncientRelicShield_OnAncientRelicShieldDepowered;
        AncientRelic.OnAncientRelicCollected -= AncientRelic_OnAncientRelicCollected;

        DialogueManager.OnDialogueStart -= DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd -= DialogueManager_OnDialogueEnd;
        MonologueManager.OnMonologueStart -= MonologueManager_OnMonologueStart;
        MonologueManager.OnMonologueEnd -= MonologueManager_OnMonologueEnd;
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

    private void ElectricalExtensibleBridge_OnExtensibleBridgePower(object sender, ElectricalExtensibleBridgeOld.OnExtensibleBridgePowerEventArgs e)
    {
        ElectricalExtensibleBridgeOld extensibleBridge = sender as ElectricalExtensibleBridgeOld;
        PlaySoundAtPoint(SFXPoolSO.extensibleBridgePowered, extensibleBridge.transform.position);
    }

    private void ElectricalExtensibleBridge_OnExtensibleBridgeDePower(object sender, ElectricalExtensibleBridgeOld.OnExtensibleBridgePowerEventArgs e)
    {
        ElectricalExtensibleBridgeOld extensibleBridge = sender as ElectricalExtensibleBridgeOld;
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

    #region Projection Selection
    private void ProjectableObjectSelectionManager_OnProjectableObjectSelected(object sender, ProjectableObjectSelectionManager.OnSelectionEventArgs e)
    {
        if (e.isFirstSelected) return;

        PlaySound(SFXPoolSO.objectSelected);

        switch (e.projectableObjectIndexed.projectableObjectSO.id)
        {
            case 1:
                PlaySound(SFXPoolSO.cableSelected);
                break;
            case 2:
                PlaySound(SFXPoolSO.magicBoxSelected);
                break;
            case 3:
                PlaySound(SFXPoolSO.senderSelected);
                break;
            case 4:
                PlaySound(SFXPoolSO.drainerSelected);
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

    private void ShieldsPopUpUI_OnShieldPopUpShow(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        PlaySound(SFXPoolSO.shieldPopUpShow);
    }

    private void ShieldsPopUpUI_OnShieldPopUpComplete(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        PlaySound(SFXPoolSO.shieldPopUpComplete);
    }

    private void ShieldsPopUpUI_OnShieldPopUpHide(object sender, ShieldsPopUpUI.OnShieldPopUpEventArgs e)
    {
        PlaySound(SFXPoolSO.shieldPopUpHide);
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

    #region Boss
    private void BossBeam_OnBeamChargeStart(object sender, BossBeam.OnBeamEventArgs e)
    {
        PlaySoundAtPoint(SFXPoolSO.bossBeamSphereCast, e.beamSphere.position);
    }

    private void BossBeam_OnBeamChargeEnd(object sender, BossBeam.OnBeamEventArgs e)
    {
        PlaySoundAtPoint(SFXPoolSO.bossBeamSphereFade, e.beamSphere.position);
    }

    private void BossBeam_OnBeamPlatformTargeted(object sender, BossBeam.OnBeamPlatformTargetEventArgs e)
    {
        PlaySoundAtPoint(SFXPoolSO.bossBeamSphereTargetLocked, e.stunableProjectionPlatformProjection.transform.position);
    }

    private void BossBeam_OnBeamPlatformStun(object sender, BossBeam.OnBeamPlatformStunEventArgs e)
    {
        PlaySoundAtPoint(SFXPoolSO.bossDematerializationLightning, e.stunableProjectionPlatformProjection.transform.position);
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        PlaySound(SFXPoolSO.bossNextPhase);
    }

    private void BossShield_OnAnyBossShieldActivated(object sender, System.EventArgs e)
    {
        BossShield bossShield = sender as BossShield;
        PlaySoundAtPoint(SFXPoolSO.bossShieldActivated, bossShield.transform.position);
    }

    private void BossShield_OnBossShieldDeactivated(object sender, System.EventArgs e)
    {
        BossShield bossShield = sender as BossShield;
        PlaySoundAtPoint(SFXPoolSO.bossShieldDeactivated, bossShield.transform.position);
    }

    private void BossStateHandler_OnBossAlmostDefeated(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.bossAlmostDefeated);
    }

    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e)
    {
        PlaySound(SFXPoolSO.bossDefeated);
    }
    #endregion

    #region Ancient Relic
    private void AncientRelicShield_OnAncientRelicShieldDepowered(object sender, System.EventArgs e)
    {
        AncientRelicShield ancientRelicShield = sender as AncientRelicShield;
        PlaySoundAtPoint(SFXPoolSO.ancientRelicShieldDepowered, ancientRelicShield.transform.position);
    }

    private void AncientRelic_OnAncientRelicCollected(object sender, System.EventArgs e)
    {
        AncientRelic ancientRelic = sender as AncientRelic;
        PlaySoundAtPoint(SFXPoolSO.ancientRelicCollected, ancientRelic.transform.position);
    }
    #endregion

    #region Showcase Rooms

    private void ShowcaseRoomBeam_OnBeamChargeStart(object sender, ShowcaseRoomBeam.OnBeamEventArgs e)
    {
        PlaySoundAtPoint(SFXPoolSO.showcaseRoomBeamSphereCast, e.beamSphere.position);
    }

    private void ShowcaseRoomBeam_OnBeamChargeEnd(object sender, ShowcaseRoomBeam.OnBeamEventArgs e)
    {
        PlaySoundAtPoint(SFXPoolSO.showcaseRoomBeamSphereFade, e.beamSphere.position);
    }

    private void ShowcaseRoomBeam_OnBeamPlatformTargeted(object sender, ShowcaseRoomBeam.OnBeamPlatformTargetEventArgs e)
    {
        PlaySoundAtPoint(SFXPoolSO.showcaseRoomBeamSphereTargetLocked, e.stunableProjectionPlatformProjection.transform.position);
    }

    private void ShowcaseRoomBeam_OnBeamPlatformStun(object sender, ShowcaseRoomBeam.OnBeamPlatformStunEventArgs e)
    {
        PlaySoundAtPoint(SFXPoolSO.showcaseRoomDematerializationLightning, e.stunableProjectionPlatformProjection.transform.position);
    }
    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeStart(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        PlaySound(SFXPoolSO.showcaseRoomNextPhase);
    }
    private void ShowcaseRoomShield_OnAnyShowcaseRoomShieldActivated(object sender, System.EventArgs e)
    {
        ShowcaseRoomShield showcaseRoomShield = sender as ShowcaseRoomShield;
        PlaySoundAtPoint(SFXPoolSO.showcaseRoomShieldActivated, showcaseRoomShield.transform.position);
    }

    private void ShowcaseRoomShield_OnAnyShowcaseRoomShieldDeactivated(object sender, System.EventArgs e)
    {
        ShowcaseRoomShield showcaseRoomShield = sender as ShowcaseRoomShield;
        PlaySoundAtPoint(SFXPoolSO.showcaseRoomShieldDeactivated, showcaseRoomShield.transform.position);
    }
    #endregion

    #region Dialogues & Monologues
    private void DialogueManager_OnDialogueStart(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        PlaySound(SFXPoolSO.dialogueOpen);
    }
    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        PlaySound(SFXPoolSO.dialogueClose);
    }
    private void MonologueManager_OnMonologueStart(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        PlaySound(SFXPoolSO.monologueOpen);
    }
    private void MonologueManager_OnMonologueEnd(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        PlaySound(SFXPoolSO.monologueClose);
    }
    #endregion
}
