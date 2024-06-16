using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerEvents : MonoBehaviour
{
    private void OnEnable()
    {
        //TUTORIAL
        GameStartCollider.OnGameStart += GameStartCollider_OnGameStart;
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

        //LEVEL1
        SecondVirtueDoorEncounterCollider.OnSecondVirtueDoorEncounter += SecondVirtueDoorEncounterCollider_OnSecondVirtueDoorEncounter;
        FirstProjectionPlatformEncounterCollider.OnFirstProjectionPlatformEncounter += FirstProjectionPlatformEncounterCollider_OnFirstProjectionPlatformEncounter;
        FirstBacktrackSwitchEncounterCollider.OnFirstBacktrackSwitchEncounter += FirstBacktrackSwitchEncounterCollider_OnFirstBacktrackSwitchEncounter;
        FirstCableRotationEncounterCollider.OnFirstCableRotationEncounter += FirstCableRotationEncounterCollider_OnFirstCableRotationEncounter;
        FirstDematerializationTowerEncounterCollider.OnFirstDematerializationTowenEncounter += FirstDematerializationTowerEncounterCollider_OnFirstDematerializationTowerEncounter;
        SecondLearningPlatformEncounterCollider.OnSecondLearningPlatformEncounter += SecondLearningPlatformEncounterCollider_OnSecondLearningPlatformEncounter;

        //LEVEL2
        ThirdVirtueDoorEncounterCollider.OnThirdVirtueDoorEncounter += ThirdVirtueDoorEncounterCollider_OnThirdVirtueDoorEncounter;
        FirstProjectionMagicBoxEncounterCollider.OnFirstProjectionMagicBoxEncounter += FirstProjectionMagicBoxEncounterCollider_OnFirstProjectionMagicBoxEncounter;
        ThirdLearningPlatformEncounterCollider.OnThirdLearningPlatformEncounter += ThirdLearningPlatformEncounterCollider_OnThirdLearningPlatformEncounter;

        //LEVEL3
        FourthVirtueDoorEncounterCollider.OnFourthVirtueDoorEncounter += FourthVirtueDoorEncounterCollider_OnFourthVirtueDoorEncounter;
        FirstProjectionSenderEncounterCollider.OnFirstProjectionSenderEncounter += FirstProjectionSenderEncounterCollider_OnFirstProjectionSenderEncounter;
        MissingLearningPlatformEncounterCollider.OnMissingLearningPlatformEncounter += MissingLearningPlatformEncounterCollider_OnMissingLearningPlatformEncounter;
    }

    private void OnDisable()
    {
        //TUTORIAL
        GameStartCollider.OnGameStart -= GameStartCollider_OnGameStart;
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

        //LEVEL1
        SecondVirtueDoorEncounterCollider.OnSecondVirtueDoorEncounter -= SecondVirtueDoorEncounterCollider_OnSecondVirtueDoorEncounter;
        FirstProjectionPlatformEncounterCollider.OnFirstProjectionPlatformEncounter -= FirstProjectionPlatformEncounterCollider_OnFirstProjectionPlatformEncounter;
        FirstBacktrackSwitchEncounterCollider.OnFirstBacktrackSwitchEncounter -= FirstBacktrackSwitchEncounterCollider_OnFirstBacktrackSwitchEncounter;
        FirstCableRotationEncounterCollider.OnFirstCableRotationEncounter -= FirstCableRotationEncounterCollider_OnFirstCableRotationEncounter;
        FirstDematerializationTowerEncounterCollider.OnFirstDematerializationTowenEncounter -= FirstDematerializationTowerEncounterCollider_OnFirstDematerializationTowerEncounter;
        SecondLearningPlatformEncounterCollider.OnSecondLearningPlatformEncounter -= SecondLearningPlatformEncounterCollider_OnSecondLearningPlatformEncounter;

        //LEVEL2
        ThirdVirtueDoorEncounterCollider.OnThirdVirtueDoorEncounter -= ThirdVirtueDoorEncounterCollider_OnThirdVirtueDoorEncounter;
        FirstProjectionMagicBoxEncounterCollider.OnFirstProjectionMagicBoxEncounter -= FirstProjectionMagicBoxEncounterCollider_OnFirstProjectionMagicBoxEncounter;
        ThirdLearningPlatformEncounterCollider.OnThirdLearningPlatformEncounter -= ThirdLearningPlatformEncounterCollider_OnThirdLearningPlatformEncounter;

        //LEVEL3
        FourthVirtueDoorEncounterCollider.OnFourthVirtueDoorEncounter -= FourthVirtueDoorEncounterCollider_OnFourthVirtueDoorEncounter;
        FirstProjectionSenderEncounterCollider.OnFirstProjectionSenderEncounter -= FirstProjectionSenderEncounterCollider_OnFirstProjectionSenderEncounter;
        MissingLearningPlatformEncounterCollider.OnMissingLearningPlatformEncounter -= MissingLearningPlatformEncounterCollider_OnMissingLearningPlatformEncounter;
    }

    //TUTORIAL
    private void GameStartCollider_OnGameStart(object sender, System.EventArgs e) => GameLogManager.Instance.Log("GameFlow/Start");
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


    //LEVEL1
    private void SecondVirtueDoorEncounterCollider_OnSecondVirtueDoorEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/SecondVirtueDoorEncounter");
    private void FirstProjectionPlatformEncounterCollider_OnFirstProjectionPlatformEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstProjectionPlatformEncounter");
    private void FirstBacktrackSwitchEncounterCollider_OnFirstBacktrackSwitchEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstBacktrackSwitchEncounter");
    private void FirstCableRotationEncounterCollider_OnFirstCableRotationEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstCableRotationEncounter");
    private void FirstDematerializationTowerEncounterCollider_OnFirstDematerializationTowerEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstDematerializationTowerEncounter");
    private void SecondLearningPlatformEncounterCollider_OnSecondLearningPlatformEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/SecondLearningPlatformEncounter");

    //LEVEL2
    private void ThirdVirtueDoorEncounterCollider_OnThirdVirtueDoorEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/ThirdVirtueDoorEncounter");
    private void FirstProjectionMagicBoxEncounterCollider_OnFirstProjectionMagicBoxEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstProjectionMagicBoxEncounter");
    private void ThirdLearningPlatformEncounterCollider_OnThirdLearningPlatformEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/ThirdLearningPlatformEncounter");

    //LEVEL3
    private void FourthVirtueDoorEncounterCollider_OnFourthVirtueDoorEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FourthVirtueDoorEncounter");
    private void FirstProjectionSenderEncounterCollider_OnFirstProjectionSenderEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/FirstProjectionSenderEncounter");
    private void MissingLearningPlatformEncounterCollider_OnMissingLearningPlatformEncounter(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Narrative/MissingLearningPlatformEncounter");
}
