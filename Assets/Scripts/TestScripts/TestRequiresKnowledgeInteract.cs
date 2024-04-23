using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TestRequiresKnowledgeInteract : MonoBehaviour, IInteractable, IRequiresDialectKnowledge
{
    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;

    [Header("Requires Knowledge Settings")]
    [SerializeField] private List<DialectKnowledge> dialectKnowledgeRequirements = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);

    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;

    public List<DialectKnowledge> DialectKnowledgeRequirements => dialectKnowledgeRequirements;

    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;

    public event EventHandler<IRequiresDialectKnowledge.OnDialectKnowledgeRequirementsNotMetEventArgs> OnDialectKnowledgeRequirementsNotMet;
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;

    #region IInteractable
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

        if (!MeetsDialectKnowledgeRequirements())
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
    public Transform GetTransform() => transform;
    #endregion

    #region IRequiresKnowledge
    public bool MeetsDialectKnowledgeRequirements()
    {
        foreach (DialectKnowledge dialectKnowledge in DialectManager.Instance.DialectKnowledges)
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
        OnDialectKnowledgeRequirementsNotMet?.Invoke(this, new IRequiresDialectKnowledge.OnDialectKnowledgeRequirementsNotMetEventArgs { dialectKnowledgeRequirements = dialectKnowledgeRequirements });
    }
    #endregion
}
