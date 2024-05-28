using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InscriptionRead : MonoBehaviour, IInteractable
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
    [Space]
    [SerializeField] private string tooltipMessageOpen;
    [SerializeField] private string tooltipMessageClose;

    public Inscription Inscription => inscription;

    private bool isOpen;

    #region IInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => $"{(!isOpen ? tooltipMessageOpen : tooltipMessageClose)}";
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
    public event EventHandler OnCloseTranslationUI;

    public event EventHandler<OnInscriptionTranslatedEventArgs> OnInscriptionTranslated;

    public static event EventHandler<OnAnyInsctiptionTranslatedEventArgs> OnAnyInscriptionTranslated;
    public class OnInscriptionTranslatedEventArgs : EventArgs
    {
        public InscriptionSO inscriptionSO;
    }

    public class OnAnyInsctiptionTranslatedEventArgs : EventArgs
    {
        public InscriptionSO inscriptionSO;
        public InscriptionRead inscriptionRead;
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

        CloseInscriptionUI();
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

        if (!isOpen)
        {
            OpenInscriptionUI();
        }
        else
        {
            CloseInscriptionUI();
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

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
        OnInscriptionTranslated?.Invoke(this, new OnInscriptionTranslatedEventArgs { inscriptionSO = inscription.InscriptionSO });
        OnAnyInscriptionTranslated?.Invoke(this, new OnAnyInsctiptionTranslatedEventArgs { inscriptionSO = inscription.InscriptionSO, inscriptionRead = this });
    }

    private void OpenInscriptionUI()
    {
        OnOpenTranslationUI?.Invoke(this, EventArgs.Empty);
        isOpen = true;

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }

    private void CloseInscriptionUI()
    {
        OnCloseTranslationUI?.Invoke(this, EventArgs.Empty);
        isOpen = false;

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }
}
