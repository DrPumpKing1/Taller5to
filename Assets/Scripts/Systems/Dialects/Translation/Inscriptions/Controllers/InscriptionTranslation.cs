using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InscriptionTranslation : MonoBehaviour, IInteractable, IRequiresSymbolCrafting
{
    [Header("Components")]
    [SerializeField] private Inscription inscription;
    [SerializeField] private SymbolCrafting symbolCrafting;

    [Header("TranslationSettings")]
    [SerializeField] private Transform inscriptionTranslationUIPrefab;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    private string tooltipMessage => $"{(!inscriptionTranslated? "Translate Insription" : "Read Inscription")}";
    private bool inscriptionTranslated;

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

    public event EventHandler OnOpenSymbolCraftingUI;
    public event EventHandler OnOpenTranslationUI;

    private void OnEnable()
    {
        symbolCrafting.OnSymbolCrafted += SymbolCrafting_OnSymbolCrafted;
    }

    private void OnDisable()
    {
        symbolCrafting.OnSymbolCrafted -= SymbolCrafting_OnSymbolCrafted;
    }

    #region  IInteractable Methods
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

    public void Interact()
    {
        //Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        if (!inscriptionTranslated)
        {
            OnOpenSymbolCraftingUI?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnOpenTranslationUI?.Invoke(this, EventArgs.Empty);
        }
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

    #region SymbolCrafting Subscriptions
    private void SymbolCrafting_OnSymbolCrafted(object sender, EventArgs e)
    {
        inscriptionTranslated = true;
        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}