using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldInteractableAlternateCrossSelectionInteractableUI : MonoBehaviour
{
    [Header("Hold Interactable Components")]
    [SerializeField] private Component holdInteractableAltenateComponent;

    [Header("UIComponents")]
    [SerializeField] private InteractableSelectionUI interactableSelectionUI;

    private IHoldInteractableAlternate holdInteractableAlternate;

    private void OnEnable()
    {
        holdInteractableAlternate.OnHoldInteractionAlternateStart += HoldInteractableAlternate_OnHoldInteractionAlternateStart;
        holdInteractableAlternate.OnHoldInteractionAlternateEnd += HoldInteractableAlternate_OnHoldInteractionAlternateEnd;

        holdInteractableAlternate.OnObjectDeselectedAlternate += HoldInteractableAlternate_OnObjectDeselectedAlternate;
    }

    private void OnDisable()
    {
        holdInteractableAlternate.OnHoldInteractionAlternateStart -= HoldInteractableAlternate_OnHoldInteractionAlternateStart;
        holdInteractableAlternate.OnHoldInteractionAlternateEnd -= HoldInteractableAlternate_OnHoldInteractionAlternateEnd;

        holdInteractableAlternate.OnObjectDeselectedAlternate -= HoldInteractableAlternate_OnObjectDeselectedAlternate;
    }

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        holdInteractableAlternate = holdInteractableAltenateComponent.GetComponent<IHoldInteractableAlternate>();
        if (holdInteractableAlternate == null) Debug.LogError("The holdInteractable component does not implement IHoldInteractable");
    }

    #region IHoldInteractable Event Subscriptions
    private void HoldInteractableAlternate_OnHoldInteractionAlternateStart(object sender, System.EventArgs e)
    {
        interactableSelectionUI.HideSelectionUI();
    }

    private void HoldInteractableAlternate_OnHoldInteractionAlternateEnd(object sender, System.EventArgs e)
    {
        if (!PlayerInteract.Instance.InteractionEnabled) return;

        interactableSelectionUI.ShowSelectionUI();
    }

    private void HoldInteractableAlternate_OnObjectDeselectedAlternate(object sender, System.EventArgs e)
    {
        interactableSelectionUI.HideSelectionUI();
    }
    #endregion
}
