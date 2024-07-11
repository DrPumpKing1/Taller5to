using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoldInteractableSelectionUI : MonoBehaviour
{
    [Header("Hold Interactable Components")]
    [SerializeField] private Component holdInteractableComponent;

    [Header("Components")]
    [SerializeField] private Animator selectionUIAnimator;
    [SerializeField] private Animator holdUIAnimator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI interactableSelectionText;
    [Space]
    [SerializeField] private TextMeshProUGUI interactableHoldText;
    [SerializeField] private Image indicatorImage;

    private IHoldInteractable holdInteractable;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        holdInteractable.OnObjectSelected += HoldInteractable_OnObjectSelected;
        holdInteractable.OnObjectDeselected += HoldInteractable_OnObjectDeselected;

        holdInteractable.OnHoldInteractionStart += HoldInteractable_OnHoldInteractionStart;
        holdInteractable.OnContinousHoldInteraction += HoldInteractable_OnContinousHoldInteraction;
        holdInteractable.OnHoldInteractionEnd += HoldInteractable_OnHoldInteractionEnd;

        holdInteractable.OnUpdatedInteractableState += HoldInteractable_OnUpdatedInteractableState;
    }

    private void OnDisable()
    {
        holdInteractable.OnObjectSelected -= HoldInteractable_OnObjectSelected;
        holdInteractable.OnObjectDeselected -= HoldInteractable_OnObjectDeselected;

        holdInteractable.OnHoldInteractionStart -= HoldInteractable_OnHoldInteractionStart;
        holdInteractable.OnContinousHoldInteraction -= HoldInteractable_OnContinousHoldInteraction;
        holdInteractable.OnHoldInteractionEnd -= HoldInteractable_OnHoldInteractionEnd;

        holdInteractable.OnUpdatedInteractableState -= HoldInteractable_OnUpdatedInteractableState;
    }

    private void Awake()
    {
        InitializeComponents();

        HideSelectionUI();
        HideHoldUI();
    }

    private void Start()
    {
        SetInteractableSelectionText();
        SetInteractableHoldText();

        GeneralUIMethods.SetImageFillRatio(indicatorImage, 0f);
    }

    private void InitializeComponents()
    {
        holdInteractable = holdInteractableComponent.GetComponent<IHoldInteractable>();
        if (holdInteractable == null) Debug.LogError("The holdInteractable component does not implement IHoldInteractable");
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

    private void HideHoldUI()
    {
        holdUIAnimator.ResetTrigger(SHOW_TRIGGER);
        holdUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    private void ShowHoldUI()
    {
        holdUIAnimator.ResetTrigger(HIDE_TRIGGER);
        holdUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    private void SetInteractableSelectionText() => interactableSelectionText.text = $"{holdInteractable.TooltipMessage}";
    private void SetInteractableHoldText() => interactableHoldText.text = $"{holdInteractable.TooltipMessage}";

    #region IHoldInteractable Event Subscriptions
    private void HoldInteractable_OnObjectSelected(object sender, System.EventArgs e)
    {
        SetInteractableSelectionText();
        SetInteractableHoldText();

        ShowSelectionUI();
    }

    private void HoldInteractable_OnObjectDeselected(object sender, System.EventArgs e)
    {
        HideSelectionUI();
        HideHoldUI();
    }

    private void HoldInteractable_OnHoldInteractionStart(object sender, System.EventArgs e)
    {
        HideSelectionUI();
        ShowHoldUI();
    }

    private void HoldInteractable_OnContinousHoldInteraction(object sender, IHoldInteractable.OnHoldInteractionEventArgs e)
    {
        GeneralUIMethods.SetImageFillRatio(indicatorImage, e.holdTimer/e.holdDuration);
    }

    private void HoldInteractable_OnHoldInteractionEnd(object sender, System.EventArgs e)
    {
        HideHoldUI();
        if(holdInteractable.IsSelectable) ShowSelectionUI();

        SetInteractableSelectionText();
        SetInteractableHoldText();
    }

    private void HoldInteractable_OnUpdatedInteractableState(object sender, System.EventArgs e)
    {
        SetInteractableSelectionText();
        SetInteractableHoldText();
    }
    #endregion
}
