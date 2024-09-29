using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerEvents : MonoBehaviour
{
    private void OnEnable()
    {
        //EVENT COLLIDERS
        EventCollider.OnEventColliderTrigger += EventCollider_OnEventColliderTrigger;

        //CINEMATICS
        CinematicsManager.OnCinematicStart += CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEnd += CinematicsManager_OnCinematicEnd;

        //TUTORIAL

        //LEVEL1

        //LEVEL2

        //LEVEL3

        //BOSS
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        AncientRelic.OnAncientRelicCollected += AncientRelic_OnAncientRelicCollected;
    }

    private void OnDisable()
    {
        //EVENT COLLIDERS
        EventCollider.OnEventColliderTrigger -= EventCollider_OnEventColliderTrigger;

        //CINEMATICS
        CinematicsManager.OnCinematicStart -= CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEnd -= CinematicsManager_OnCinematicEnd;

        //TUTORIAL

        //LEVEL1

        //LEVEL2

        //LEVEL3


        //BOSS
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        AncientRelic.OnAncientRelicCollected -= AncientRelic_OnAncientRelicCollected;
    }

    //EVENT COLLIDERS
    private void EventCollider_OnEventColliderTrigger(object sender, EventCollider.OnEventColliderTriggerEventArgs e) => GameLogManager.Instance.Log($"Events/EventCollider/{e.eventID}");

    //CINEMATICS
    private void CinematicsManager_OnCinematicStart(object sender, CinematicsManager.OnCinematicEventArgs e) => GameLogManager.Instance.Log($"Cinematics/Start/{e.cinematic.id}");
    private void CinematicsManager_OnCinematicEnd(object sender, CinematicsManager.OnCinematicEventArgs e) => GameLogManager.Instance.Log($"Cinematics/End/{e.cinematic.id}");

    //TUTORIAL

    //LEVEL1

    //LEVEL3

    //BOSS
    private void BossStateHandler_OnPlayerDefeated(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Events/PlayerDefeated");
    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Events/BossDefeated");
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e) => GameLogManager.Instance.Log($"Events/BossPhaseChange/{e.phaseNumber}");
    private void AncientRelic_OnAncientRelicCollected(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Events/AncientRelicCollected");
}
