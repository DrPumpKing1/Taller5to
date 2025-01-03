using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatformLearn : MonoBehaviour, IHoldInteractable
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;
    [SerializeField] private Transform projectionGemCenter;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f, 100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private float holdDuration;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;
    [SerializeField] private Transform interactionAttentionTransform;
    [SerializeField] private Transform interactionPositionTransform;

    #region IHoldInteractableProperties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public float HoldDuration => holdDuration;
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;

    #endregion

    public ProjectableObjectSO ProjectableObjectToLearn => learningPlatform.LearningPlatformSO.projectableObjectToLearn;

    #region IHoldInteractable Events
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;

    public event EventHandler OnHoldInteractionStart;
    public event EventHandler OnHoldInteractionEnd;
    public event EventHandler<IHoldInteractable.OnHoldInteractionEventArgs> OnContinousHoldInteraction;
    #endregion

    public event EventHandler<OnObjectLearnedEventArgs> OnObjectLearned;
    public event EventHandler<OnObjectLearnedEventArgs> OnObjectAlreadyLearned;

    public static event EventHandler<OnLearningEventArgs> OnStartLearning;
    public static event EventHandler<OnLearningEventArgs> OnEndLearning;

    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableOjectSO;
    }

    public class OnLearningEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableOjectSO;
        public Transform interactionAttentionTransform;
        public Transform projectionGemCenter;
        public float holdDuration;
    }

    private void Start()
    {
        CheckIsAlreadyLearned();
    }

    private void CheckIsAlreadyLearned()
    {
        if (learningPlatform.ObjectHasBeenLearned())
        {
            DisableInteractability();
            OnObjectAlreadyLearned?.Invoke(this, new OnObjectLearnedEventArgs { projectableOjectSO = ProjectableObjectToLearn });
        }
    }

    #region IHoldInteractable Methods
    public void Select() => OnObjectSelected?.Invoke(this, EventArgs.Empty);
    public void Deselect() => OnObjectDeselected?.Invoke(this, EventArgs.Empty);

    public void TryInteract()
    {
        if (!isInteractable)
        {
            FailInteract();
            return;
        }

        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        Interact();
    }

    public void Interact()
    {
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        LearnObject();
    }

    public void FailInteract()
    {
        Debug.Log("Cant Interact with Learning Platform");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void AlreadyInteracted()
    {
        Debug.Log("Learning Platform has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
    }

    public bool CheckSuccess()
    {
        if (!isInteractable)
        {
            FailInteract();
            return false;
        }

        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return false;
        }

        return true;
    }

    public void HoldInteractionStart()
    {
        OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
        OnStartLearning?.Invoke(this, new OnLearningEventArgs { projectableOjectSO = ProjectableObjectToLearn,interactionAttentionTransform = interactionAttentionTransform, projectionGemCenter = projectionGemCenter, holdDuration = holdDuration });
    }
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });

    public void HoldInteractionEnd()
    {
        OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);
        OnEndLearning?.Invoke(this, new OnLearningEventArgs { projectableOjectSO = ProjectableObjectToLearn, interactionAttentionTransform = interactionAttentionTransform, projectionGemCenter = projectionGemCenter, holdDuration = holdDuration });
    }

    public Transform GetTransform() => transform;
    public Transform GetInteractionAttentionTransform() => interactionAttentionTransform;
    public Transform GetInteractionPositionTransform() => interactionPositionTransform;
    #endregion

    private void LearnObject()
    {
        DisableInteractability();

        AddObjectToLearnedList();
        AddProjectionGems();

        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { projectableOjectSO = ProjectableObjectToLearn });
        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }

    private void DisableInteractability()
    {
        canBeSelected = false;
        isInteractable = false;
        hasAlreadyBeenInteracted = true;
    }

    private void AddObjectToLearnedList() => ProjectableObjectsLearningManager.Instance.LearnProjectableObject(ProjectableObjectToLearn, learningPlatform);
    private void AddProjectionGems() => ProjectionGemsManager.Instance.IncreaseTotalProjectionGems(learningPlatform.LearningPlatformSO.projectionGemsToAdd);
}
