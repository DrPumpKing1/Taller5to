using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
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

    #region IInteractable
    public void Interact()
    {
        Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        DecreaseUses();
    }

    public void FailInteract()
    {
        Debug.Log(gameObject.name + " Fail Interacted");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void OnDeselection() => Debug.Log(gameObject.name + " Deselected");

    public void OnSelection() => Debug.Log(gameObject.name + " Selected");

    public void TryInteract()
    {
        if (IsInteractable) Interact();
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
