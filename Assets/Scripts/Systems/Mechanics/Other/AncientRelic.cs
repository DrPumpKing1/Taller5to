using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AncientRelic : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] private Transform model;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f, 100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [Space]
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;
    [SerializeField] private Transform attentionTransform;

    #region IInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;
    public Transform AttentionTransform => attentionTransform;
    #endregion

    #region IInteractableEvents
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;
    #endregion

    public static event EventHandler OnAncientRelicCollected;

    #region  IInteractable Methods
    public void Interact()
    {
        //Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        CollectAncientRelic();
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

    private void CollectAncientRelic()
    {
        canBeSelected = false;
        isInteractable = false;
        hasAlreadyBeenInteracted = true;

        DisableModel();
        OnAncientRelicCollected?.Invoke(this, EventArgs.Empty);

        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }

    private void DisableModel() => model.gameObject.SetActive(false);
}
