using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestRequiresKnowledgeHoldInteract : MonoBehaviour, IHoldInteractable, IHoldInteractableAlternate, IRequiresKnowledge
{
    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private float holdDuration;

    [Header("Interactable Alternate Settings")]
    [SerializeField] private bool canBeSelectedAlternate;
    [SerializeField] private bool isInteractableAlternate;
    [SerializeField] private bool hasAlreadyBeenInteractedAlternate;
    [SerializeField] private string tooltipMessageAlternate;
    [Space]
    [SerializeField] private float holdDurationAlternate;

    [Header("Requires Knowledge Settings")]
    [SerializeField] private List<DialectKnowledge> dialectKnowledgeRequirements = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);

    #region IHoldInteractable Properties
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public float HoldDuration => holdDuration;
    #endregion

    #region IHoldInteractableAlternate Properties
    public bool IsSelectableAlternate => canBeSelectedAlternate;
    public bool IsInteractableAlternate => IsInteractableAlternate;
    public bool HasAlreadyBeenInteractedAlternate => hasAlreadyBeenInteractedAlternate;
    public string TooltipMessageAlternate => tooltipMessageAlternate;
    public float HoldDurationAlternate => holdDurationAlternate;
    #endregion

    #region  IRequiresKnowledge Properties
    public List<DialectKnowledge> DialectKnowledgeRequirements => dialectKnowledgeRequirements;
    #endregion

    #region IHoldInteractableEvents
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;

    public event EventHandler OnHoldInteractionStart;
    public event EventHandler OnHoldInteractionEnd;
    public event EventHandler<IHoldInteractable.OnHoldInteractionEventArgs> OnContinousHoldInteraction;
    #endregion

    #region IHoldInteractable Alternate Events
    public event EventHandler OnObjectSelectedAlternate;
    public event EventHandler OnObjectDeselectedAlternate;
    public event EventHandler OnObjectInteractedAlternate;
    public event EventHandler OnObjectFailInteractedAlternate;
    public event EventHandler OnObjectHasAlreadyBeenInteractedAlternate;

    public event EventHandler OnHoldInteractionAlternateStart;
    public event EventHandler OnHoldInteractionAlternateEnd;
    public event EventHandler<IHoldInteractableAlternate.OnHoldInteractionAlternateEventArgs> OnContinousHoldInteractionAlternate;
    #endregion

    #region IRequiresKnowledge Events
    public event EventHandler<IRequiresKnowledge.OnKnowledgeRequirementsNotMetEventArgs> OnKnowledgeRequirementsNotMet;
    #endregion

    #region IHoldInteractable Methods
    public void Select()
    {
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        Debug.Log(gameObject.name + " Selected");
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        Debug.Log(gameObject.name + " Deselected");
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

        if (!MeetsKnowledgeRequirements())
        {
            KnowledgeRequirementsNotMet();
            return;
        }

        Interact();
    }
    public void Interact()
    {
        Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        hasAlreadyBeenInteracted = true;
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

        if (!MeetsKnowledgeRequirements())
        {
            KnowledgeRequirementsNotMet();
            return false;
        }

        return true;
    }
    public void HoldInteractionStart() => OnHoldInteractionStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteraction(float holdTimer) => OnContinousHoldInteraction?.Invoke(this, new IHoldInteractable.OnHoldInteractionEventArgs { holdTimer = holdTimer, holdDuration = holdDuration });
    public void HoldInteractionEnd() => OnHoldInteractionEnd?.Invoke(this, EventArgs.Empty);

    public Transform GetTransform() => transform;
    #endregion

    #region IHoldInteractableAlternate Methods
    public void SelectAlternate()
    {
        OnObjectSelectedAlternate?.Invoke(this, EventArgs.Empty);
        Debug.Log(gameObject.name + " Selected Alternate");
    }

    public void DeselectAlternate()
    {
        OnObjectDeselectedAlternate?.Invoke(this, EventArgs.Empty);
        Debug.Log(gameObject.name + " Deselected Alternate");
    }

    public void TryInteractAlternate()
    {
        if (!isInteractableAlternate)
        {
            FailInteractAlternate();
            return;
        }

        if (hasAlreadyBeenInteractedAlternate)
        {
            AlreadyInteractedAlternate();
            return;
        }

        if (!MeetsKnowledgeRequirements())
        {
            KnowledgeRequirementsNotMet();
            return;
        }

        InteractAlternate();
    }

    public void InteractAlternate()
    {
        Debug.Log(gameObject.name + " Interacted Alternate");
        OnObjectInteractedAlternate?.Invoke(this, EventArgs.Empty);

        hasAlreadyBeenInteractedAlternate = true;
    }

    public void FailInteractAlternate()
    {
        Debug.Log(gameObject.name + " Fail Interacted Alternate");
        OnObjectFailInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }

    public void AlreadyInteractedAlternate()
    {
        Debug.Log(gameObject.name + " Has Already Been Interacted Alternate");
        OnObjectHasAlreadyBeenInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }

    public bool CheckSuccessAlternate()
    {
        if (!isInteractableAlternate)
        {
            FailInteractAlternate();
            return false;
        }

        if (hasAlreadyBeenInteractedAlternate)
        {
            AlreadyInteractedAlternate();
            return false;
        }

        if (!MeetsKnowledgeRequirements())
        {
            KnowledgeRequirementsNotMet();
            return false;
        }

        return true;
    }

    public void HoldInteractionAlternateStart() => OnHoldInteractionAlternateStart?.Invoke(this, EventArgs.Empty);
    public void ContinousHoldInteractionAlternate(float holdTimer) => OnContinousHoldInteractionAlternate?.Invoke(this, new IHoldInteractableAlternate.OnHoldInteractionAlternateEventArgs { holdTimer = holdTimer, holdDuration = holdDurationAlternate });
    public void HoldInteractionAlternateEnd() => OnHoldInteractionAlternateEnd?.Invoke(this, EventArgs.Empty);

    #endregion

    #region IRequiresKnowledge Methods
    public bool MeetsKnowledgeRequirements()
    {
        foreach (DialectKnowledge dialectKnowledge in KnowledgeManager.Instance.GetDialectKnowledges())
        {
            foreach (DialectKnowledge dialectKnowledgeRequirement in dialectKnowledgeRequirements)
            {
                if (dialectKnowledge.dialect == dialectKnowledgeRequirement.dialect)
                {
                    if (dialectKnowledge.level < dialectKnowledgeRequirement.level) return false;
                }
            }
        }

        return true;
    }
    public void KnowledgeRequirementsNotMet()
    {
        Debug.Log(gameObject.name + " knowledge requirements not met");
        OnKnowledgeRequirementsNotMet?.Invoke(this, new IRequiresKnowledge.OnKnowledgeRequirementsNotMetEventArgs { dialectKnowledgeRequirements = dialectKnowledgeRequirements });
    }
    #endregion

}
