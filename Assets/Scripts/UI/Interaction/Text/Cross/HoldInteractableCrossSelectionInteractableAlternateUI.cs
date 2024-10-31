using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldInteractableCrossSelectionInteractableAlternateUI : MonoBehaviour
{
    [Header("Hold Interactable Components")]
    [SerializeField] private Component holdInteractableComponent;

    [Header("UIComponents")]
    [SerializeField] private InteractableAlternateSelectionUI interactableAlternateSelectionUI;

    private IHoldInteractable holdInteractable;

    private void OnEnable()
    {
        holdInteractable.OnHoldInteractionStart += HoldInteractable_OnHoldInteractionStart;
        holdInteractable.OnHoldInteractionEnd += HoldInteractable_OnHoldInteractionEnd;

        holdInteractable.OnObjectDeselected += HoldInteractable_OnObjectDeselected;
    }

    private void OnDisable()
    {
        holdInteractable.OnHoldInteractionStart -= HoldInteractable_OnHoldInteractionStart;
        holdInteractable.OnHoldInteractionEnd -= HoldInteractable_OnHoldInteractionEnd;

        holdInteractable.OnObjectDeselected -= HoldInteractable_OnObjectDeselected;
    }

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        holdInteractable = holdInteractableComponent.GetComponent<IHoldInteractable>();
        if (holdInteractable == null) Debug.LogError("The holdInteractable component does not implement IHoldInteractable");
    }

    #region IHoldInteractable Event Subscriptions
    private void HoldInteractable_OnHoldInteractionStart(object sender, System.EventArgs e)
    {
        interactableAlternateSelectionUI.HideSelectionUI();
    }

    private void HoldInteractable_OnHoldInteractionEnd(object sender, System.EventArgs e)
    {
        if (!PlayerInteractAlternate.Instance.InteractionAlternateEnabled) return;

        interactableAlternateSelectionUI.ShowSelectionUI();
    }

    private void HoldInteractable_OnObjectDeselected(object sender, System.EventArgs e)
    {
        interactableAlternateSelectionUI.HideSelectionUI();
    }
    #endregion
}
