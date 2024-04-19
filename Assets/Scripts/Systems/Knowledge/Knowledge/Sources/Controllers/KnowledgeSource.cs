using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeSource : MonoBehaviour, IHoldInteractable
{
    [Header ("Knowledge Source Settings")]
    [SerializeField] private KnowledgeSourceSO knowledgeSourceSO;
    [SerializeField, Range (0f,1f)] private float destroyTime;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;
    [SerializeField] private float holdDuration;

    #region IInteractable Properties
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;

    public float HoldDuration => holdDuration;
    #endregion

    #region IInteractableEvents
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    #endregion

    public event EventHandler<OnKnowledgeAddedEventArgs> OnKnowledgeAdded;
    public event EventHandler OnHoldInteractionStart;
    public event EventHandler OnHoldInteractionEnd;
    public event EventHandler<IHoldInteractable.OnHoldInteractionEventArgs> OnContinousHoldInteraction;

    public class OnKnowledgeAddedEventArgs : EventArgs
    {
        public KnowledgeSourceSO knowledgeSourceSO;
    }

    #region  IInteractable Methods
    public void Interact()
    {
        //Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        hasAlreadyBeenInteracted = true;
        isInteractable = false;
        canBeSelected = false;

        AddKnowledgeToDialects();
        DestroyKnowledgeSource();
    }

    public void FailInteract()
    {
        Debug.Log(gameObject.name + " Fail Interacted");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void AlreadyInteracted()
    {
        Debug.Log(gameObject.name + " Has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void Select()
    {
        //Enable some UI feedback
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        Debug.Log(gameObject.name + " Selected");
    }

    public void Deselect()
    {
        //Disable some UI feedback
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        Debug.Log(gameObject.name + " Deselected");
    }

    public void TryInteract()
    {
        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        if (!isInteractable)
        {
            FailInteract();
            return;
        }

        Interact();
    }

    public Transform GetTransform() => transform;

    #endregion

    private void AddKnowledgeToDialects()
    {
        foreach (DialectKnowledge dialectKnowledgePercentageChange in knowledgeSourceSO.dialectKnowledgeLevelChanges)
        {
            KnowledgeManager.Instance.ChangeKnowledge(dialectKnowledgePercentageChange.dialect, dialectKnowledgePercentageChange.level);
        }

        OnKnowledgeAdded?.Invoke(this, new OnKnowledgeAddedEventArgs { knowledgeSourceSO = knowledgeSourceSO });
    }

    private void DestroyKnowledgeSource() => Destroy(gameObject, destroyTime);

    public bool CheckSuccess()
    {
        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return false;
        }

        if (!isInteractable)
        {
            FailInteract();
            return false;
        }

        return true;
    }

    public void HoldInteractionStart() => OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionEnd() => OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);

}
