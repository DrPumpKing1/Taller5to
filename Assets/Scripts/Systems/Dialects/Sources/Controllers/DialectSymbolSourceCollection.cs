using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialectSymbolSourceCollection : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] private DialectSymbolSource dialectSymbolSource;
    [SerializeField] private Transform visual;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f,100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f,100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [Space]
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;

    #region IInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;
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

    private void Start()
    {
        CheckIsCollected();
    }

    private void CheckIsCollected()
    {
        if (!dialectSymbolSource.IsCollected) return;

        DisableVisual();
        canBeSelected = false;
        isInteractable = false;
        hasAlreadyBeenInteracted = true;
    }

    #region  IInteractable Methods
    public void Interact()
    {
        //Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        CollectSymbolSource();
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
    }

    public void Deselect()
    {
        //Disable some UI feedback
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
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

    private void CollectSymbolSource()
    {
        AddSymbolsToInventory();
        DisableVisual();

        dialectSymbolSource.SetIsCollected();

        canBeSelected = false;
        isInteractable = false;
        hasAlreadyBeenInteracted = true;
    }

    private void DisableVisual() => visual.gameObject.SetActive(false);

    private void AddSymbolsToInventory()
    {
        foreach (DialectSymbolSO dialectSymbolSO in dialectSymbolSource.DialectSymbolSourceSO.dialectSymbolSOs)
        {
            SymbolsDictionaryManager.Instance.CollectSymbol(dialectSymbolSO);
        }

        OnSymbolsAdded?.Invoke(this, new OnSymbolsAddedEventArgs { dialectSymbolSourceSO = dialectSymbolSource.DialectSymbolSourceSO });
    }
}
