using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableAlternateSelectionUI : MonoBehaviour
{
    [Header("Interactable Alternate Components")]
    [SerializeField] private Component interactableAlternateComponent;

    [Header("Components")]
    [SerializeField] private Animator selectionUIAnimator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI interactableAlternateSelectionText;

    private IInteractableAlternate interactableAlternate;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        interactableAlternate.OnObjectSelectedAlternate += InteractableAlternate_OnObjectSelectedAlternate;
        interactableAlternate.OnObjectDeselectedAlternate += InteractableAlternate_OnObjectDeselectedAlternate;
        interactableAlternate.OnObjectInteractedAlternate += InteractableAlternate_OnObjectInteractedAlternate;
        interactableAlternate.OnUpdatedInteractableAlternateState += InteractableAlternate_OnUpdatedInteractableAlternateState;
    }

    private void OnDisable()
    {
        interactableAlternate.OnObjectSelectedAlternate -= InteractableAlternate_OnObjectSelectedAlternate;
        interactableAlternate.OnObjectDeselectedAlternate -= InteractableAlternate_OnObjectDeselectedAlternate;
        interactableAlternate.OnObjectInteractedAlternate -= InteractableAlternate_OnObjectInteractedAlternate;
        interactableAlternate.OnUpdatedInteractableAlternateState -= InteractableAlternate_OnUpdatedInteractableAlternateState;
    }

    private void Awake()
    {
        InitializeComponents();
        HideSelectionUI();
    }

    private void Start()
    {
        SetInteractableAlternateSelectionText();
    }

    private void InitializeComponents()
    {
        interactableAlternate = interactableAlternateComponent.GetComponent<IInteractableAlternate>();
        if (interactableAlternate == null) Debug.LogError("The interactableAlternate component does not implement IInteractableAlternate");
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

    private void SetInteractableAlternateSelectionText() => interactableAlternateSelectionText.text = $"{interactableAlternate.TooltipMessageAlternate}";

    #region IInteractableAlternate Event Subscriptions
    private void InteractableAlternate_OnObjectSelectedAlternate(object sender, System.EventArgs e)
    {
        SetInteractableAlternateSelectionText();
        ShowSelectionUI();
    }

    private void InteractableAlternate_OnObjectDeselectedAlternate(object sender, System.EventArgs e)
    {
        HideSelectionUI();
    }

    private void InteractableAlternate_OnObjectInteractedAlternate(object sender, System.EventArgs e)
    {
        SetInteractableAlternateSelectionText();
    }

    private void InteractableAlternate_OnUpdatedInteractableAlternateState(object sender, System.EventArgs e)
    {
        SetInteractableAlternateSelectionText();
    }
    #endregion
}
