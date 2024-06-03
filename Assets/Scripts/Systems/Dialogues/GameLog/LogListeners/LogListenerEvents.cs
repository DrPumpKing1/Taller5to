using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerEvents : MonoBehaviour
{
    private void OnEnable()
    {
        //EVENTS
        GameStartEvent.OnGameStart += GameStartEvent_OnGameStart;
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
    }



    private void OnDisable()
    {
        //EVENTS
        GameStartEvent.OnGameStart -= GameStartEvent_OnGameStart;
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
    }

    private void GameStartEvent_OnGameStart(object sender, System.EventArgs e) => GameLogManager.Instance.Log("GameFlow/Start");
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
}
