using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatformLearn : MonoBehaviour, IHoldInteractable
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;
    [SerializeField] private Transform rotatingObject;

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

    [Header("Proximity Learn")]
    [SerializeField] private bool enableProximityLearn;
    [SerializeField] private float proximityRadius;

    private GameObject player;

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

    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableOjectSO;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckProximityLearn();
    }

    private void InitializeVariables()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void CheckProximityLearn()
    {
        if (!enableProximityLearn) return;
        if (learningPlatform.IsLearned) return;
        if (!(Vector3.Distance(transform.position, player.transform.position) <= proximityRadius)) return;

        LearnObject();
    }

    #region IHoldInteractable Methods
    public void Select()
    {
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        Debug.Log("Learning Platform Selected");
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        Debug.Log("Learning Platform Deselected");
    }
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

        canBeSelected = false;
        isInteractable = false;
        hasAlreadyBeenInteracted = true;

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
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

    public void HoldInteractionStart() => OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionEnd() => OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);

    public Transform GetTransform() => transform;
    #endregion
    private void DisableRotatingObject() => rotatingObject.gameObject.SetActive(false); 

    private void LearnObject()
    {
        ProjectableObjectsLearningManager.Instance.LearnProjectableObject(ProjectableObjectToLearn);
        ProjectionGemsManager.Instance.IncreaseTotalProjectionGems(learningPlatform.LearningPlatformSO.projectionGemsToAdd);

        learningPlatform.SetIsLearned(true);
        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { projectableOjectSO = ProjectableObjectToLearn });

        DisableRotatingObject();
    }
}
