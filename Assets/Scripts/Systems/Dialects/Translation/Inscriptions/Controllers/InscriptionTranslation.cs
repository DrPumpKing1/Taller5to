using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InscriptionTranslation : MonoBehaviour, IInteractable, IRequiresSymbolCrafting
{
    [Header("Components")]
    [SerializeField] private Inscription inscription;
    [SerializeField] private SymbolCrafting symbolCrafting;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f, 100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;

    private string tooltipMessage => $"{(!inscriptionTranslated? "Translate Insription" : "Read Inscription")}";
    private bool inscriptionTranslated;

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

    public event EventHandler OnOpenSymbolCraftingUI;
    public event EventHandler OnOpenTranslationUI;
    public event EventHandler OnInscriptionTranslated;

    private void OnEnable()
    {
        symbolCrafting.OnSymbolsCrafted += SymbolCrafting_OnSymbolsCrafted;
    }

    private void OnDisable()
    {
        symbolCrafting.OnSymbolsCrafted -= SymbolCrafting_OnSymbolsCrafted;
    }

    private void Start()
    {
        CheckIsTranslated();
    }

    private void CheckIsTranslated()
    {
        if (inscription.IsTranslated) inscriptionTranslated = true;
        if (symbolCrafting.SymbolCraftingSOs.Count == 0) inscriptionTranslated = true;
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
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        if (!inscriptionTranslated)
        {
            OpenSymbolCraftingUI();
        }
        else
        {
            OpenTranslationUI();
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

    private void TranslateInscription()
    {
        inscriptionTranslated = true;

        inscription.SetTranslated();

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
        OnInscriptionTranslated?.Invoke(this, EventArgs.Empty);
    }

    private void OpenSymbolCraftingUI() => OnOpenSymbolCraftingUI?.Invoke(this, EventArgs.Empty);
    private void OpenTranslationUI() => OnOpenTranslationUI?.Invoke(this, EventArgs.Empty);

    #region SymbolCrafting Subscriptions
    private void SymbolCrafting_OnSymbolsCrafted(object sender, EventArgs e)
    {
        TranslateInscription();
    }
    #endregion
}
