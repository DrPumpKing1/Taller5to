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

    [Space]
    [SerializeField] private bool infiniteUses;
    [SerializeField, Range(1, 100)] private int useTimes;
    private int remainingUses;

    [Space]
    [SerializeField] private string tooltipMessage;

    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public string TooltipMessage => tooltipMessage;
    public bool InfiniteUses => infiniteUses;
    public int UseTimes => useTimes;

    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        remainingUses = useTimes;
    }


    private void AddKnowledgeToDialects() 
    { 
        foreach(DialectKnowledge dialectKnowledgePercentageChange in knowledgeSourceSO.dialectKnowledgePercentageChanges)
        {
            KnowledgeManager.Instance.ChangeKnowledge(dialectKnowledgePercentageChange.dialect, dialectKnowledgePercentageChange.percentage);
        }
    }

    #region  IInteractable
    public void Interact()
    {
        AddKnowledgeToDialects();

        Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        DecreaseUses();
    }

    public void FailInteract()
    {
        Debug.Log(gameObject.name + " Fail Interacted");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void OnSelection()
    {
        //Enable some UI feedback
        Debug.Log(gameObject.name + " Selected");
    }

    public void OnDeselection()
    {
        //Disable some UI feedback
        Debug.Log(gameObject.name + " Deselected");
    }

    public void TryInteract()
    {
        if (isInteractable) Interact();
        else FailInteract();
    }

    public void DecreaseUses()
    {
        if (infiniteUses) return;

        remainingUses--;

        if (remainingUses <= 0) isInteractable = false;
    }

    public Transform GetTransform() => transform;

    #endregion
}
