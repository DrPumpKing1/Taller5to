using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatformLearn : MonoBehaviour, IInteractable, IRequiresSymbolCrafting
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;
    [SerializeField] private SymbolCrafting symbolCrafting;

    [Header("Interactable Settings")]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [SerializeField] private string tooltipMessage;

    #region IInteractableProperties
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    #endregion

    #region IInteractable Events
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;
    #endregion

    public event EventHandler OnOpenSymbolCraftingUI;

    private void OnEnable()
    {
        symbolCrafting.OnSymbolsCrafted += SymbolCrafting_OnSymbolCrafted;
    }

    private void OnDisable()
    {
        symbolCrafting.OnSymbolsCrafted -= SymbolCrafting_OnSymbolCrafted;
    }

    #region IInteractable Methods
    public void Select()
    {
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
        Debug.Log("Learning Platform Selected");
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
        Debug.Log("Learning Platform Deselected");
    }
    public void TryInteract()
    {
        if (!isInteractable)
        {
            FailInteract();
            return;
        }

        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        Interact();
    }

    public void Interact()
    {
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        if (!symbolCrafting.SymbolsCrafted)
        {
            OnOpenSymbolCraftingUI?.Invoke(this, EventArgs.Empty);
        }
    }

    public void FailInteract()
    {
        Debug.Log("Cant Interact with Learning Platform");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }
    public void AlreadyInteracted()
    {
        Debug.Log("Learning Platform has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetTransform() => transform;
    #endregion

    #region SymbolCrafting Subscriptions
    private void SymbolCrafting_OnSymbolCrafted(object sender, EventArgs e)
    {
        canBeSelected = false;
        isInteractable = false;
    }
    #endregion
}
