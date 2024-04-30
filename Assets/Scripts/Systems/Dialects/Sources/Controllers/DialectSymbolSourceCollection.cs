using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialectSymbolSourceCollection : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] private DialectSymbolSource dialectSymbolSource;

    [Header("Symbol Source Collection Settings")]
    [SerializeField, Range(0f, 1f)] private float destroyTime;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;

    #region IInteractable Properties
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    #endregion

    #region IInteractableEvents
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;
    #endregion

    public event EventHandler<OnSymbolsAddedEventArgs> OnSymbolsAdded;

    public class OnSymbolsAddedEventArgs : EventArgs
    {
        public DialectSymbolsSourceSO dialectSymbolSourceSO;
    }

    #region  IInteractable Methods
    public void Interact()
    {
        //Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        hasAlreadyBeenInteracted = true;
        isInteractable = false;
        canBeSelected = false;

        AddSymbolsToInventory();
        DestroyDialectSymbolSource();
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
    private void AddSymbolsToInventory()
    {
        foreach (DialectSymbolSO dialectSymbolSO in dialectSymbolSource.DialectSymbolSourceSO.dialectSymbolSOs)
        {
            SymbolsDictionaryManager.Instance.AddSymbolToDictionary(dialectSymbolSO);
        }

        OnSymbolsAdded?.Invoke(this, new OnSymbolsAddedEventArgs { dialectSymbolSourceSO = dialectSymbolSource.DialectSymbolSourceSO });
    }

    private void DestroyDialectSymbolSource() => Destroy(gameObject, destroyTime);
}
