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

        //CAMERA TRANSITION
        CameraTransitionHandler.OnCameraTransitionInStart += CameraTransitionHandler_OnCameraTransitionInStart;
        CameraTransitionHandler.OnCameraTransitionOutStart += CameraTransitionHandler_OnCameraTransitionOutStart;

        //TUTORIAL

        //LEVEL1

        //LEVEL2

        //LEVEL3

        //BOSS
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
        AncientRelic.OnAncientRelicCollected += AncientRelic_OnAncientRelicCollected;

        //VYRX
        PetPlayerAttachment.OnVyrxInitialAttachToPlayer += PetPlayerAttachment_OnVyrxInitialAttachToPlayer;
    }

    private void OnDisable()
    {
        //EVENT COLLIDERS
        EventCollider.OnEventColliderTrigger -= EventCollider_OnEventColliderTrigger;

        //CINEMATICS
        CinematicsManager.OnCinematicStart -= CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEnd -= CinematicsManager_OnCinematicEnd;

        //CAMERA TRANSITION
        CameraTransitionHandler.OnCameraTransitionInStart -= CameraTransitionHandler_OnCameraTransitionInStart;
        CameraTransitionHandler.OnCameraTransitionOutStart -= CameraTransitionHandler_OnCameraTransitionOutStart;

        //TUTORIAL

        //LEVEL1

        //LEVEL2

        //LEVEL3


        //BOSS
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        AncientRelic.OnAncientRelicCollected -= AncientRelic_OnAncientRelicCollected;

        //VYRX
        PetPlayerAttachment.OnVyrxInitialAttachToPlayer -= PetPlayerAttachment_OnVyrxInitialAttachToPlayer;
    }

    //EVENT COLLIDERS
    private void EventCollider_OnEventColliderTrigger(object sender, EventCollider.OnEventColliderTriggerEventArgs e) => GameLogManager.Instance.Log($"Events/EventCollider/{e.eventID}");

    //CINEMATICS
    private void CinematicsManager_OnCinematicStart(object sender, CinematicsManager.OnCinematicEventArgs e) => GameLogManager.Instance.Log($"Cinematics/Start/{e.cinematic.id}");
    private void CinematicsManager_OnCinematicEnd(object sender, CinematicsManager.OnCinematicEventArgs e) => GameLogManager.Instance.Log($"Cinematics/End/{e.cinematic.id}");


    //CAMERA TRANSITIONS
    private void CameraTransitionHandler_OnCameraTransitionInStart(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e) => GameLogManager.Instance.Log($"CameraTransition/In/Start/{e.cameraTransition.id}");
    private void CameraTransitionHandler_OnCameraTransitionOutStart(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e) => GameLogManager.Instance.Log($"CameraTransition/Out/Start/{e.cameraTransition.id}");


    //TUTORIAL

    //LEVEL1

    //LEVEL3

    //BOSS
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e) => GameLogManager.Instance.Log($"Events/BossPhaseChange/Start/{e.nextPhase}");
    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e) => GameLogManager.Instance.Log($"Events/BossPhaseChange/End/{e.nextPhase}");

    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Events/BossDefeated");
    private void AncientRelic_OnAncientRelicCollected(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Events/AncientRelicCollected");

    //VYRX
    private void PetPlayerAttachment_OnVyrxInitialAttachToPlayer(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Events/Vyrx/InitialAttachToPlayer");
}
