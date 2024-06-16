using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerInteraction : MonoBehaviour
{
    private void OnEnable()
    {
        //INTERACTION
        InscriptionRead.OnInscriptionRead += InscriptionRead_OnInscriptionRead;
        VisualInteractableElement.OnVisualInteractableElementInteracted += VisualInteractableElement_OnVisualInteractableElementInteracted;
        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;

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
        VisualInteractableElement.OnVisualInteractableElementInteracted -= VisualInteractableElement_OnVisualInteractableElementInteracted;
        ShieldPiecesManager.OnShieldPieceCollected -= ShieldPiecesManager_OnShieldPieceCollected;

        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;
        ProjectionGemsManager.OnTotalProjectionGemsIncreased -= ProjectionGemsManager_OnTotalProjectionGemsIncreased;

        ProjectableObjectRotation.OnAnyObjectRotated -= ProjectableObjectRotation_OnAnyObjectRotated;
        ProjectableObjectActivation.OnAnyObjectActivated -= ProjectableObjectActivation_OnAnyObjectActivated;
        ProjectableObjectActivation.OnAnyObjectDeactivated -= ProjectableObjectActivation_OnAnyObjectDeactivated;
    }

    private void InscriptionRead_OnInscriptionRead(object sender, InscriptionRead.OnInscriptionReadEventArgs e) => GameLogManager.Instance.Log($"Interaction/InscriptionRead/{e.inscriptionSO.id}");
    private void VisualInteractableElement_OnVisualInteractableElementInteracted(object sender, VisualInteractableElement.OnVisualInteractableElementInteractedEventArgs e) => GameLogManager.Instance.Log($"Interaction/VisualInteractableElementInteracted/{e.id}");
    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e) => GameLogManager.Instance.Log($"Interaction/CollectShieldPiece/{e.shieldPieceSO.id}");
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
