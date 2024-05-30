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

        ShieldPieceCollection.OnShieldPieceCollected += ShieldPieceCollection_OnShieldPieceCollected;
        LearningPlatformLearn.OnAnyObjectLearned += LearningPlatformLearn_OnAnyObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased += ProjectionGemsManager_OnTotalProjectionGemsIncreased;                    

        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;

    }



    private void OnDisable()
    {
        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;

        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
        ElectricalDoor.OnDoorPowered -= ElectricalDoor_OnDoorPowered;

        RoomManager.OnRoomEnter -= RoomManager_OnRoomEnter;
        RoomManager.OnRoomExit -= RoomManager_OnRoomExit;

        InscriptionPowering.OnInscriptionPoweringFirstTime -= InscriptionPowering_OnInscriptionPoweringFirstTime;
        ShieldPieceCollection.OnShieldPieceCollected -= ShieldPieceCollection_OnShieldPieceCollected;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;

        LearningPlatformLearn.OnAnyObjectLearned += LearningPlatformLearn_OnAnyObjectLearned;

    }


    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e) => GameLog.Log($"Electrical/ToggleSwitch/{e.switchOn}/{e.id}");
    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e) => GameLog.Log($"Electrical/PowerDoor/{e.id}");
    private void InscriptionPowering_OnInscriptionPoweringFirstTime(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e) => GameLog.Log($"Electrical/PowerInscription/{e.id}");

    private void RoomManager_OnRoomEnter(object sender, RoomManager.OnRoomEventArgs e) => GameLog.Log($"Movement/EnterRoom/{e.roomName}");
    private void RoomManager_OnRoomExit(object sender, RoomManager.OnRoomEventArgs e) => GameLog.Log($"Movement/ExitRoom/{e.roomName}");

    private void ShieldPieceCollection_OnShieldPieceCollected(object sender, ShieldPieceCollection.OnShieldPieceCollectedEventArgs e) => GameLog.Log($"Interaction/GrabShieldPiece/{e.shieldPieceSO.id}");
    private void LearningPlatformLearn_OnAnyObjectLearned(object sender, LearningPlatformLearn.OnAnyObjectLearnedEventArgs e) => GameLog.Log($"Interaction/LearnObject/{e.projectableObjectSO.id}");
    private void ProjectionGemsManager_OnTotalProjectionGemsIncreased(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e) => GameLog.Log($"Interaction/GrabGems/{e.projectionGems}");

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e) => GameLog.Log($"Worth/ShowDignity/{e.dialect}");



}
