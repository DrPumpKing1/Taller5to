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

        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased += ProjectionGemsManager_OnTotalProjectionGemsIncreased;

        ProjectableObjectRotation.OnAnyObjectRotated += ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated += ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated += ProjectableObjectActivation_OnAnyObjectDeactivated;
    }

    private void OnDisable()
    {
        //PROJECTION
        InscriptionRead.OnInscriptionRead -= InscriptionRead_OnInscriptionRead;
        NarrativeElement.OnNarrativeElementInteracted -= NarrativeElement_OnNarrativeElementInteracted;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;

        ProjectableObjectRotation.OnAnyObjectRotated -= ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated -= ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated -= ProjectableObjectActivation_OnAnyObjectDeactivated;
    }

    private void InscriptionRead_OnInscriptionRead(object sender, InscriptionRead.OnInscriptionReadEventArgs e) => GameLogManager.Instance.Log($"Interaction/InscriptionRead/{e.inscriptionSO.id}");
    private void NarrativeElement_OnNarrativeElementInteracted(object sender, NarrativeElement.OnCommonInteractableElementInteractedEventArgs e) => GameLogManager.Instance.Log($"Interaction/NarrativeElement/{e.id}");
    private void ProjectableObjectsLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e) => GameLogManager.Instance.Log($"Interaction/LearnObject/{e.projectableObjectLearned.id}");
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
}
