using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListener : MonoBehaviour
{
    private void OnEnable()
    {
        ElectricalSwitchToggle.OnSwitchToggle += ElectricalSwitchToggle_OnSwitchToggle;
        ElectricalDoor.OnDoorPowered += ElectricalDoor_OnDoorPowered;
        InscriptionPowering.OnInscriptionPoweringFirstTime += InscriptionPowering_OnInscriptionPoweringFirstTime;

        RoomManager.OnRoomEnter += RoomManager_OnRoomEnter;
        RoomManager.OnRoomExit += RoomManager_OnRoomExit;

        InscriptionRead.OnInscriptionRead += InscriptionRead_OnInscriptionRead;
        ShieldPieceCollection.OnShieldPieceCollected += ShieldPieceCollection_OnShieldPieceCollected;
        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased += ProjectionGemsManager_OnTotalProjectionGemsIncreased;
        ProjectableObjectRotation.OnAnyObjectRotated += ProjectableObjectRotation_OnAnyObjectRotated;

        ProjectionManager.OnObjectProjectionSuccess += ProjectionManager_OnObjectProjectionSuccess;
        ProjectionManager.OnObjectProjectionFailed += ProjectionManager_OnObjectProjectionFailed;
        ProjectionManager.OnObjectDematerialized += ProjectionManager_OnObjectDematerialized;
        ProjectionManager.OnAllObjectsDematerialized += ProjectionManager_OnAllObjectsDematerialized;


        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
    }

    private void OnDisable()
    {
        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;
        ElectricalDoor.OnDoorPowered -= ElectricalDoor_OnDoorPowered;
        InscriptionPowering.OnInscriptionPoweringFirstTime -= InscriptionPowering_OnInscriptionPoweringFirstTime;

        RoomManager.OnRoomEnter -= RoomManager_OnRoomEnter;
        RoomManager.OnRoomExit -= RoomManager_OnRoomExit;

        InscriptionRead.OnInscriptionRead -= InscriptionRead_OnInscriptionRead;
        ShieldPieceCollection.OnShieldPieceCollected -= ShieldPieceCollection_OnShieldPieceCollected;
        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;
        ProjectableObjectRotation.OnAnyObjectRotated -= ProjectableObjectRotation_OnAnyObjectRotated;

        ProjectionManager.OnObjectProjectionSuccess -= ProjectionManager_OnObjectProjectionSuccess;
        ProjectionManager.OnObjectProjectionFailed -= ProjectionManager_OnObjectProjectionFailed;
        ProjectionManager.OnObjectDematerialized -= ProjectionManager_OnObjectDematerialized;
        ProjectionManager.OnAllObjectsDematerialized -= ProjectionManager_OnAllObjectsDematerialized;

        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
    }

    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e) => GameLog.Log($"Electrical/ToggleSwitch/{e.switchOn}/{e.id}");
    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e) => GameLog.Log($"Electrical/PowerDoor/{e.id}");
    private void InscriptionPowering_OnInscriptionPoweringFirstTime(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e) => GameLog.Log($"Electrical/PowerInscription/{e.id}");


    private void RoomManager_OnRoomEnter(object sender, RoomManager.OnRoomEventArgs e) => GameLog.Log($"Movement/EnterRoom/{e.roomName}");
    private void RoomManager_OnRoomExit(object sender, RoomManager.OnRoomEventArgs e) => GameLog.Log($"Movement/ExitRoom/{e.roomName}");


    private void InscriptionRead_OnInscriptionRead(object sender, InscriptionRead.OnInscriptionReadEventArgs e) => GameLog.Log($"Interaction/InscriptionRead/{e.inscriptionSO.id}");
    private void ShieldPieceCollection_OnShieldPieceCollected(object sender, ShieldPieceCollection.OnShieldPieceCollectedEventArgs e) => GameLog.Log($"Interaction/GrabShieldPiece/{e.shieldPieceSO.id}");
    private void ProjectableObjectsLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e) => GameLog.Log($"Interaction/LearnObject/{e.projectableObjectLearned.id}");
    private void ProjectionGemsManager_OnTotalProjectionGemsIncreased(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e) => GameLog.Log($"Interaction/GrabGems/{e.projectionGems}");
    private void ProjectableObjectRotation_OnAnyObjectRotated(object sender, ProjectableObjectRotation.OnAnyObjectRotatedEventArgs e) => GameLog.Log($"Interaction/RotateObject/{e.projectableObjectSO.id}");


    private void ProjectionManager_OnObjectProjectionSuccess(object sender, ProjectionManager.OnProjectionEventArgs e) => GameLog.Log($"Projection/MaterializeObject/{e.projectableObjectSO.id}");
    private void ProjectionManager_OnObjectProjectionFailed(object sender, ProjectionManager.OnProjectionEventArgs e) => GameLog.Log($"Projection/FailMaterializeObject/{e.projectableObjectSO.id}");
    private void ProjectionManager_OnObjectDematerialized(object sender, ProjectionManager.OnProjectionEventArgs e) => GameLog.Log($"Projection/DematerializeObject/{e.projectableObjectSO.id}");
    private void ProjectionManager_OnAllObjectsDematerialized(object sender, ProjectionManager.OnAllObjectsDematerializedEventArgs e) => GameLog.Log($"Projection/DematerializeAllObjects/{e.projectableObjectSOs.Count}");


    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e) => GameLog.Log($"Worth/ShowDignity/{e.dialect}");
}
