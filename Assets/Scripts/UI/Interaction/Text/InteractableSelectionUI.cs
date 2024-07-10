using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableSelectionUI : MonoBehaviour
{
    [Header("Interactable Components")]
    [SerializeField] private Component interactableComponent;

    [Header("Components")]
    [SerializeField] private Animator selectionUIAnimator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI interactableSelectionText;

    private IInteractable interactable;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        interactable.OnObjectSelected += Interactable_OnObjectSelected;
        interactable.OnObjectDeselected += Interactable_OnObjectDeselected;
        interactable.OnObjectInteracted += Interactable_OnObjectInteracted;
        interactable.OnUpdatedInteractableState += Interactable_OnUpdatedInteractableState;
    }

    private void OnDisable()
    {
        interactable.OnObjectSelected -= Interactable_OnObjectSelected;
        interactable.OnObjectDeselected -= Interactable_OnObjectDeselected;
        interactable.OnObjectInteracted -= Interactable_OnObjectInteracted;
        interactable.OnUpdatedInteractableState -= Interactable_OnUpdatedInteractableState;
    }

    private void Awake()
    {
        InitializeComponents();
        HideSelectionUI();
    }

    private void Start()
    {
        SetInteractableSelectionText();
    }

    private void InitializeComponents()
    {
        interactable = interactableComponent.GetComponent<IInteractable>();
        if (interactable == null) Debug.LogError("The interactable component does not implement IInteractable");
    }

    public void HideSelectionUI()
    {
        selectionUIAnimator.ResetTrigger(SHOW_TRIGGER);
        selectionUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    public void ShowSelectionUI()
    {
        selectionUIAnimator.ResetTrigger(HIDE_TRIGGER);
        selectionUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    private void SetInteractableSelectionText() => interactableSelectionText.text = $"{interactable.TooltipMessage}";

    #region IInteractable Event Subscriptions
    private void Interactable_OnObjectSelected(object sender, System.EventArgs e)
    {
        SetInteractableSelectionText();
        ShowSelectionUI();
    }

    private void Interactable_OnObjectDeselected(object sender, System.EventArgs e)
    {
        HideSelectionUI();
    }

    private void Interactable_OnObjectInteracted(object sender, System.EventArgs e)
    {
        SetInteractableSelectionText();
    }

    private void Interactable_OnUpdatedInteractableState(object sender, System.EventArgs e)
    {
        SetInteractableSelectionText();
    }

    #endregion
}
