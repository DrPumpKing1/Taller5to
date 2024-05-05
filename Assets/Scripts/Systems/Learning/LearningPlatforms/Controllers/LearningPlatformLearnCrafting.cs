using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatformLearnCrafting : MonoBehaviour, IInteractable, IRequiresSymbolCrafting
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;
    [SerializeField] private SymbolCrafting symbolCrafting;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f, 100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [Space]
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;

    #region IInteractableProperties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;
    #endregion

    #region IInteractable Events
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;
    #endregion

    public event EventHandler OnOpenSymbolCraftingUI;
    public event EventHandler<OnObjectLearnedEventArgs> OnObjectLearned;
    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO objectLearned;
    }

    private void OnEnable()
    {
        symbolCrafting.OnSymbolsCrafted += SymbolCrafting_OnSymbolCrafted;
    }

    private void OnDisable()
    {
        symbolCrafting.OnSymbolsCrafted -= SymbolCrafting_OnSymbolCrafted;
    }

    private void Start()
    {
        CheckIsLearned();
    }

    private void CheckIsLearned()
    {
        if (!learningPlatform.IsLearned) return;
        
        canBeSelected = false;
        isInteractable = false;
        hasAlreadyBeenInteracted = true;  
    }

    #region IInteractable Methods
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

        if (!symbolCrafting.AllSymbolsCrafted)
        {
            OnOpenSymbolCraftingUI?.Invoke(this, EventArgs.Empty);
        }
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

    public Transform GetTransform() => transform;
    #endregion

    public void LearnObject()
    {
        ProjectableObjectsLearningManager.Instance.LearnProjectableObject(learningPlatform.ProjectableObjectToLearn); ;

        learningPlatform.SetIsLearned();

        canBeSelected = false;
        isInteractable = false;
        hasAlreadyBeenInteracted = true;

        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { objectLearned = learningPlatform.ProjectableObjectToLearn });
    }

    #region SymbolCrafting Subscriptions
    private void SymbolCrafting_OnSymbolCrafted(object sender, EventArgs e)
    {
        LearnObject();
    }
    #endregion
}
