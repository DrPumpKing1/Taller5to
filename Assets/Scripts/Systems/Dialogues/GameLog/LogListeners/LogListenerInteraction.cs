using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerInteraction : MonoBehaviour
{
    private void OnEnable()
    {
        //INTERACTION
        InscriptionRead.OnInscriptionRead += InscriptionRead_OnInscriptionRead;
        NarrativeElement.OnNarrativeElementInteracted += NarrativeElement_OnNarrativeElementInteracted;
        TeleportationObject.OnTeleportationObjectInteracted += TeleportationObject_OnTeleportationObjectInteracted;

        LearningPlatformLearn.OnStartLearning += LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning += LearningPlatformLearn_OnEndLearning;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased += ProjectionGemsManager_OnTotalProjectionGemsIncreased;

        ProjectableObjectRotation.OnAnyObjectRotated += ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated += ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated += ProjectableObjectActivation_OnAnyObjectDeactivated;

        ProjectionResetObject.OnAnyProjectionResetObjectUsed += ProjectionResetObject_OnAnyProjectionResetObjectUsed;
    }


    private void OnDisable()
    {
        //INTERACTION
        InscriptionRead.OnInscriptionRead -= InscriptionRead_OnInscriptionRead;
        NarrativeElement.OnNarrativeElementInteracted -= NarrativeElement_OnNarrativeElementInteracted;
        TeleportationObject.OnTeleportationObjectInteracted -= TeleportationObject_OnTeleportationObjectInteracted;

        LearningPlatformLearn.OnStartLearning -= LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning -= LearningPlatformLearn_OnEndLearning;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;

        ProjectableObjectRotation.OnAnyObjectRotated -= ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated -= ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated -= ProjectableObjectActivation_OnAnyObjectDeactivated;

        ProjectionResetObject.OnAnyProjectionResetObjectUsed -= ProjectionResetObject_OnAnyProjectionResetObjectUsed;
    }

    private void InscriptionRead_OnInscriptionRead(object sender, InscriptionRead.OnInscriptionReadEventArgs e) => GameLogManager.Instance.Log($"Interaction/InscriptionRead/{e.inscriptionSO.id}");
    private void NarrativeElement_OnNarrativeElementInteracted(object sender, NarrativeElement.OnNarrativeElementInteractedEventArgs e) => GameLogManager.Instance.Log($"Interaction/NarrativeElement/{e.id}");
    private void TeleportationObject_OnTeleportationObjectInteracted(object sender, TeleportationObject.OnTeleportationObjectInteractedEventArgs e) => GameLogManager.Instance.Log($"Interaction/TeleportationObject/{e.id}");

    private void LearningPlatformLearn_OnStartLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        GameLogManager.Instance.Log($"Interaction/Learning/Start");
        GameLogManager.Instance.Log($"Interaction/LearningExact/Start/{e.projectableOjectSO.id}");
    }
    private void LearningPlatformLearn_OnEndLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        GameLogManager.Instance.Log($"Interaction/Learning/End");
        GameLogManager.Instance.Log($"Interaction/LearningExact/End/{e.projectableOjectSO.id}");
    }

    private void ProjectableObjectsLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e) =>GameLogManager.Instance.Log($"Interaction/LearnObject/{e.projectableObjectLearned.id}");
    private void ProjectionGemsManager_OnTotalProjectionGemsIncreased(object sender, ProjectionGemsManager.OnProjectionGemsEventArgs e) => GameLogManager.Instance.Log($"Interaction/GrabGems/{e.projectionGems}");

    private void ProjectableObjectRotation_OnAnyObjectRotated(object sender, ProjectableObjectRotation.OnAnyObjectRotatedEventArgs e)
    {
        GameLogManager.Instance.Log($"Interaction/RotateObject/{e.projectableObjectSO.id}");
        GameLogManager.Instance.Log($"Interaction/RotateObjectExact/{e.projectableObjectSO.id}/{e.projectionPlatformID}");
    }
    private void ProjectableObjectActivation_OnAnyObjectActivated(object sender, ProjectableObjectActivation.OnAnyObjectActivatedEventArgs e)
    {
        GameLogManager.Instance.Log($"Interaction/ActivateObject/{e.projectableObjectSO.id}");
        GameLogManager.Instance.Log($"Interaction/ActivateObjectExact/{e.projectableObjectSO.id}/{e.projectionPlatformID}");
    }
    private void ProjectableObjectActivation_OnAnyObjectDeactivated(object sender, ProjectableObjectActivation.OnAnyObjectActivatedEventArgs e)
    {
        GameLogManager.Instance.Log($"Interaction/DeactivateObject/{e.projectableObjectSO.id}");
        GameLogManager.Instance.Log($"Interaction/DeactivateObjectExact/{e.projectableObjectSO.id}/{e.projectionPlatformID}");
    }

    private void ProjectionResetObject_OnAnyProjectionResetObjectUsed(object sender, ProjectionResetObject.OnProjectionResetObjectEventArgs e)
    {
        GameLogManager.Instance.Log($"Interaction/ProjectionResetObjectUsed");
        GameLogManager.Instance.Log($"Interaction/ProjectionResetObjectUsedExact/{e.id}");
    }
}
