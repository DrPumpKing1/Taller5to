using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeSource : MonoBehaviour, IInteractable
{
    [SerializeField] private KnowledgeSourceSO knowledgeSourceSO;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;

    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;

    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;

    private void AddKnowledgeToDialects() 
    { 
        foreach(DialectKnowledge dialectKnowledgePercentageChange in knowledgeSourceSO.dialectKnowledgePercentageChanges)
        {
            KnowledgeManager.Instance.ChangeKnowledge(dialectKnowledgePercentageChange.dialect, dialectKnowledgePercentageChange.level);
        }
    }

    #region  IInteractable
    public void Interact()
    {
        AddKnowledgeToDialects();

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

    public void Select()
    {
        //Enable some UI feedback
        Debug.Log(gameObject.name + " Selected");
    }

    public void Deselect()
    {
        //Disable some UI feedback
        Debug.Log(gameObject.name + " Deselected");
    }

    public void TryInteract()
    {
        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        if (isInteractable) Interact();
        else FailInteract();
    }

    public Transform GetTransform() => transform;

    #endregion
}
