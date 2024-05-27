using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InscriptionTranslation : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] private Inscription inscription;

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

    public Inscription Inscription => inscription;

    #region IInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => $"{(!inscription.IsTranslated ? "Translate Insription" : "Read Inscription")}";
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

    public event EventHandler OnOpenTranslationUI;
    public event EventHandler<OnInsctiptionTranslatedEventArgs> OnInscriptionTranslated;

    public static event EventHandler<OnAnyInsctiptionTranslatedEventArgs> OnAnyInscriptionTranslated;
    public class OnInsctiptionTranslatedEventArgs : EventArgs
    {
        public InscriptionSO inscriptionSO;
    }

    public class OnAnyInsctiptionTranslatedEventArgs : EventArgs
    {
        public InscriptionSO inscriptionSO;
        public InscriptionTranslation inscriptionTranslation;
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

        if (!inscription.IsTranslated)
        {
            OpenTranslationUI();
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
        inscription.SetIsTranslated();

        inscription.SetIsTranslated();

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
        OnInscriptionTranslated?.Invoke(this, new OnInsctiptionTranslatedEventArgs { inscriptionSO = inscription.InscriptionSO });
        OnAnyInscriptionTranslated?.Invoke(this, new OnAnyInsctiptionTranslatedEventArgs { inscriptionSO = inscription.InscriptionSO, inscriptionTranslation = this });
    }

    private void OpenTranslationUI() => OnOpenTranslationUI?.Invoke(this, EventArgs.Empty);
}
