using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerEvents : MonoBehaviour
{
    private void OnEnable()
    {
        //EVENTS
        GameStartEvent.OnGameStart += GameStartEvent_OnGameStart;
        BackToShipCollider.OnBackToShip += BackToShipCollider_OnBackToShip;
        MeetVyrxCollider.OnMeetVyrx += MeetVyrxCollider_OnMeetVyrx;
        IntroduceVyrxCollider.OnIntroduceVyrx += IntroduceVyrxCollider_OnIntroduceVyrx;
        FirstKaerumEncounterCollider.OnFirstKaerumEncounter += FirstKaerumEncounterCollider_OnFirstKaerumEncounter;
        FirstKaerumSwitchEncounterCollider.OnFirstKaerumSwitchEncounter += FirstKaerumSwitchEncounterCollider_OnFirstKaerumSwitchEncounter;
        FirstKaerumRemoteSwitchEncounterCollider.OnFirstKaerumRemoteSwitchEncounter += FirstKaerumRemoteSwitchEncounterCollider_OnFirstKaerumRemoteSwitchEncounter;
        SecondKaerumEncounterCollider.OnSecondKaerumEncounter += SecondKaerumEncounterCollider_OnSecondKaerumEncounter;
        FirstKaerumDoubleSwitchEncounterCollider.OnFirstKaerumDoubleSwitchEncounter += FirstKaerumDoubleSwitchEncounterCollider_OnFirstKaerumDoubleSwitchEncounter;
        FirstVirtueDoorEncounterCollider.OnFirstVirtueDoorEncounter += FirstVirtueDoorEncounterCollider_OnFirstVirtueDoorEncounter;
        FirstInscriptionEncounterCollider.OnFirstInscriptionEncounter += FirstInscriptionEncounterCollider_OnFirstInscriptionEncounter;
        FirstLearningPlatformEncounterCollider.OnFirstLearningPlatformEncounter += FirstLearningPlatformEncounterCollider_OnFirstLearningPlatformEncounter;

        SecondVirtueDoorEncounterCollider.OnSecondVirtueDoorEncounter += SecondVirtueDoorEncounterCollider_OnSecondVirtueDoorEncounter;
        FirstBackTrackInscriptionEncounterCollider.OnFirstBackTrackInscriptionEncounter += FirstBackTrackInscriptionEncounterCollider_OnFirstBackTrackInscriptionEncounter;
        FirstProjectionPlatformEncounterCollider.OnFirstProjectionPlatformEncounter += FirstProjectionPlatformEncounterCollider_OnFirstProjectionPlatformEncounter;
        FirstDematerializationTowerEncounterCollider.OnFirstDematerializationTowerEncounter += FirstDematerializationTowerEncounterCollider_OnFirstDematerializationTowerEncounter;
        FirstCableRotationEncounterCollider.OnFirstCableRotationEncounter += FirstCableRotationEncounterCollider_OnFirstCableRotationEncounter;
        SecondLearningPlatformEncounterCollider.OnSecondLearningPlatformEncounter += SecondLearningPlatformEncounterCollider_OnSecondLearningPlatformEncounter;
    }

    private void OnDisable()
    {
        //EVENTS
        GameStartEvent.OnGameStart -= GameStartEvent_OnGameStart;
        BackToShipCollider.OnBackToShip -= BackToShipCollider_OnBackToShip;
        MeetVyrxCollider.OnMeetVyrx -= MeetVyrxCollider_OnMeetVyrx;
        IntroduceVyrxCollider.OnIntroduceVyrx -= IntroduceVyrxCollider_OnIntroduceVyrx;
        FirstKaerumEncounterCollider.OnFirstKaerumEncounter -= FirstKaerumEncounterCollider_OnFirstKaerumEncounter;
        FirstKaerumSwitchEncounterCollider.OnFirstKaerumSwitchEncounter -= FirstKaerumSwitchEncounterCollider_OnFirstKaerumSwitchEncounter;
        FirstKaerumRemoteSwitchEncounterCollider.OnFirstKaerumRemoteSwitchEncounter -= FirstKaerumRemoteSwitchEncounterCollider_OnFirstKaerumRemoteSwitchEncounter;
        SecondKaerumEncounterCollider.OnSecondKaerumEncounter -= SecondKaerumEncounterCollider_OnSecondKaerumEncounter;
        FirstKaerumDoubleSwitchEncounterCollider.OnFirstKaerumDoubleSwitchEncounter -= FirstKaerumDoubleSwitchEncounterCollider_OnFirstKaerumDoubleSwitchEncounter;
        FirstVirtueDoorEncounterCollider.OnFirstVirtueDoorEncounter -= FirstVirtueDoorEncounterCollider_OnFirstVirtueDoorEncounter;
        FirstInscriptionEncounterCollider.OnFirstInscriptionEncounter -= FirstInscriptionEncounterCollider_OnFirstInscriptionEncounter;
        FirstLearningPlatformEncounterCollider.OnFirstLearningPlatformEncounter -= FirstLearningPlatformEncounterCollider_OnFirstLearningPlatformEncounter;

        SecondVirtueDoorEncounterCollider.OnSecondVirtueDoorEncounter -= SecondVirtueDoorEncounterCollider_OnSecondVirtueDoorEncounter;
        FirstBackTrackInscriptionEncounterCollider.OnFirstBackTrackInscriptionEncounter -= FirstBackTrackInscriptionEncounterCollider_OnFirstBackTrackInscriptionEncounter;
        FirstProjectionPlatformEncounterCollider.OnFirstProjectionPlatformEncounter -= FirstProjectionPlatformEncounterCollider_OnFirstProjectionPlatformEncounter;
        FirstDematerializationTowerEncounterCollider.OnFirstDematerializationTowerEncounter -= FirstDematerializationTowerEncounterCollider_OnFirstDematerializationTowerEncounter;
        FirstCableRotationEncounterCollider.OnFirstCableRotationEncounter -= FirstCableRotationEncounterCollider_OnFirstCableRotationEncounter;
        SecondLearningPlatformEncounterCollider.OnSecondLearningPlatformEncounter -= SecondLearningPlatformEncounterCollider_OnSecondLearningPlatformEncounter;
    }

    private void GameStartEvent_OnGameStart(object sender, System.EventArgs e) => GameLogManager.Instance.Log("GameFlow/Start");
    private void BackToShipCollider_OnBackToShip(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/BackToShip");
    private void MeetVyrxCollider_OnMeetVyrx(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/MeetVyrx");
    private void IntroduceVyrxCollider_OnIntroduceVyrx(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/IntroduceVyrx");
    private void FirstKaerumEncounterCollider_OnFirstKaerumEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstKaerumEncounter");
    private void FirstKaerumSwitchEncounterCollider_OnFirstKaerumSwitchEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstKaerumSwitchEncounter");
    private void FirstKaerumRemoteSwitchEncounterCollider_OnFirstKaerumRemoteSwitchEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstKaerumRemoteSwitchEncounter");
    private void SecondKaerumEncounterCollider_OnSecondKaerumEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/SecondKaerumSwitchEncounter");
    private void FirstKaerumDoubleSwitchEncounterCollider_OnFirstKaerumDoubleSwitchEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstKaerumDoubleSwitchEncounter");
    private void FirstVirtueDoorEncounterCollider_OnFirstVirtueDoorEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstVirtueDoorEncounter");
    private void FirstInscriptionEncounterCollider_OnFirstInscriptionEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstInscriptionEncounter");
    private void FirstLearningPlatformEncounterCollider_OnFirstLearningPlatformEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstLearningPlatformEncounter");

    private void SecondVirtueDoorEncounterCollider_OnSecondVirtueDoorEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/SecondVirtueDoorEncounter");
    private void FirstBackTrackInscriptionEncounterCollider_OnFirstBackTrackInscriptionEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstBacktrackInscriptionEncounter");
    private void FirstProjectionPlatformEncounterCollider_OnFirstProjectionPlatformEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstProjectionPlatformEncounter");
    private void FirstDematerializationTowerEncounterCollider_OnFirstDematerializationTowerEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstDematerializationTowerEncounter");
    private void FirstCableRotationEncounterCollider_OnFirstCableRotationEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstCableRotationEncounter");
    private void SecondLearningPlatformEncounterCollider_OnSecondLearningPlatformEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/SecondLearningPlatformEncounter");

}
