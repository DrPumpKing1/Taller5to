using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListener : MonoBehaviour
{
    private void OnEnable()
    {
        //ELECTRICAL
        ElectricalSwitchToggle.OnSwitchToggle += ElectricalSwitchToggle_OnSwitchToggle;
        ElectricalDoor.OnDoorPowered += ElectricalDoor_OnDoorPowered;
        InscriptionPowering.OnInscriptionPoweringFirstTime += InscriptionPowering_OnInscriptionPoweringFirstTime;

        //ROOMS
        RoomManager.OnRoomEnter += RoomManager_OnRoomEnter;
        RoomManager.OnRoomExit += RoomManager_OnRoomExit;

        //INTERACTION
        InscriptionRead.OnInscriptionRead += InscriptionRead_OnInscriptionRead;
        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;
        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased += ProjectionGemsManager_OnTotalProjectionGemsIncreased;
        ProjectableObjectRotation.OnAnyObjectRotated += ProjectableObjectRotation_OnAnyObjectRotated;

        //PROJECTION
        ProjectionManager.OnObjectProjectionSuccess += ProjectionManager_OnObjectProjectionSuccess;
        ProjectionManager.OnObjectProjectionFailed += ProjectionManager_OnObjectProjectionFailed;
        ProjectionManager.OnObjectDematerialized += ProjectionManager_OnObjectDematerialized;
        ProjectionManager.OnAllObjectsDematerialized += ProjectionManager_OnAllObjectsDematerialized;

        //SHIELDS
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;

        //EVENTS
        GameStartEvent.OnGameStart += GameStartEvent_OnGameStart;
        MeetVyrxCollider.OnMeetVyrx += MeetVyrxCollider_OnMeetVyrx;
    }

    private void OnDisable()
    {
        //ELECTRICAL
        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;
        ElectricalDoor.OnDoorPowered -= ElectricalDoor_OnDoorPowered;
        InscriptionPowering.OnInscriptionPoweringFirstTime -= InscriptionPowering_OnInscriptionPoweringFirstTime;

        //INTERACTION
        RoomManager.OnRoomEnter -= RoomManager_OnRoomEnter;
        RoomManager.OnRoomExit -= RoomManager_OnRoomExit;

        //PROJECTION
        InscriptionRead.OnInscriptionRead -= InscriptionRead_OnInscriptionRead;
        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;
        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;
        ProjectableObjectRotation.OnAnyObjectRotated -= ProjectableObjectRotation_OnAnyObjectRotated;

        //SHIELDS
        ProjectionManager.OnObjectProjectionSuccess -= ProjectionManager_OnObjectProjectionSuccess;
        ProjectionManager.OnObjectProjectionFailed -= ProjectionManager_OnObjectProjectionFailed;
        ProjectionManager.OnObjectDematerialized -= ProjectionManager_OnObjectDematerialized;
        ProjectionManager.OnAllObjectsDematerialized -= ProjectionManager_OnAllObjectsDematerialized;

        //SHIELDS
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;

        //EVENTS
        GameStartEvent.OnGameStart -= GameStartEvent_OnGameStart;
        MeetVyrxCollider.OnMeetVyrx -= MeetVyrxCollider_OnMeetVyrx;
    }

    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e) => GameLogManager.Instance.Log($"Electrical/ToggleSwitch/{e.switchOn}/{e.id}");
    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerDoor/{e.id}");
    private void InscriptionPowering_OnInscriptionPoweringFirstTime(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerInscription/{e.id}");


    private void RoomManager_OnRoomEnter(object sender, RoomManager.OnRoomEventArgs e) => GameLogManager.Instance.Log($"Movement/EnterRoom/{e.roomName}");
    private void RoomManager_OnRoomExit(object sender, RoomManager.OnRoomEventArgs e) => GameLogManager.Instance.Log($"Movement/ExitRoom/{e.roomName}");


    private void InscriptionRead_OnInscriptionRead(object sender, InscriptionRead.OnInscriptionReadEventArgs e) => GameLogManager.Instance.Log($"Interaction/InscriptionRead/{e.inscriptionSO.id}");
    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e) => GameLogManager.Instance.Log($"Interaction/CollectShieldPiece/{e.shieldPieceSO.id}");
    private void ProjectableObjectsLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e) => GameLogManager.Instance.Log($"Interaction/LearnObject/{e.projectableObjectLearned.id}");
    private void ProjectionGemsManager_OnTotalProjectionGemsIncreased(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e) => GameLogManager.Instance.Log($"Interaction/GrabGems/{e.projectionGems}");
    private void ProjectableObjectRotation_OnAnyObjectRotated(object sender, ProjectableObjectRotation.OnAnyObjectRotatedEventArgs e) => GameLogManager.Instance.Log($"Interaction/RotateObject/{e.projectableObjectSO.id}");


    private void ProjectionManager_OnObjectProjectionSuccess(object sender, ProjectionManager.OnProjectionEventArgs e) => GameLogManager.Instance.Log($"Projection/MaterializeObject/{e.projectableObjectSO.id}");
    private void ProjectionManager_OnObjectProjectionFailed(object sender, ProjectionManager.OnProjectionEventArgs e) => GameLogManager.Instance.Log($"Projection/FailMaterializeObject/{e.projectableObjectSO.id}");
    private void ProjectionManager_OnObjectDematerialized(object sender, ProjectionManager.OnProjectionEventArgs e) => GameLogManager.Instance.Log($"Projection/DematerializeObject/{e.projectableObjectSO.id}");
    private void ProjectionManager_OnAllObjectsDematerialized(object sender, ProjectionManager.OnAllObjectsDematerializedEventArgs e) => GameLogManager.Instance.Log($"Projection/DematerializeAllObjects/{e.projectableObjectSOs.Count}");


    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e) => GameLogManager.Instance.Log($"Worth/ShowDignity/{e.dialect}");

    private void GameStartEvent_OnGameStart(object sender, System.EventArgs e) => GameLogManager.Instance.Log("GameFlow/Start");
    private void MeetVyrxCollider_OnMeetVyrx(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/MeetVyrx");
}
