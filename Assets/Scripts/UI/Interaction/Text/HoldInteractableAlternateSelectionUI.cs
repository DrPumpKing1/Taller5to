using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoldInteractableAlternateSelectionUI : MonoBehaviour
{
    [Header("Hold Interactable Alternate Components")]
    [SerializeField] private Component holdInteractableAlternateComponent;

    [Header("Components")]
    [SerializeField] private Animator selectionUIAnimator;
    [SerializeField] private Animator holdUIAnimator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI interactableAlternateSelectionText;
    [Space]
    [SerializeField] private TextMeshProUGUI interactableAlternateHoldText;
    [SerializeField] private Image indicatorImage;

    private IHoldInteractableAlternate holdInteractableAlternate;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        holdInteractableAlternate.OnObjectSelectedAlternate += HoldInteractableAlternate_OnObjectSelected;
        holdInteractableAlternate.OnObjectDeselectedAlternate += HoldInteractableAlternate_OnObjectDeselected;

        holdInteractableAlternate.OnHoldInteractionAlternateStart += HoldInteractableAlternate_OnHoldInteractionAlternateStart;
        holdInteractableAlternate.OnContinousHoldInteractionAlternate += HoldInteractableAlternate_OnContinousHoldInteractionAlternate;
        holdInteractableAlternate.OnHoldInteractionAlternateEnd += HoldInteractableAlternate_OnHoldInteractionAlternateEnd;

        holdInteractableAlternate.OnUpdatedInteractableAlternateState += HoldInteractableAlternate_OnUpdatedInteractableAlternateState;
    }

    private void OnDisable()
    {
        holdInteractableAlternate.OnObjectSelectedAlternate -= HoldInteractableAlternate_OnObjectSelected;
        holdInteractableAlternate.OnObjectDeselectedAlternate -= HoldInteractableAlternate_OnObjectDeselected;

        holdInteractableAlternate.OnHoldInteractionAlternateStart -= HoldInteractableAlternate_OnHoldInteractionAlternateStart;
        holdInteractableAlternate.OnContinousHoldInteractionAlternate -= HoldInteractableAlternate_OnContinousHoldInteractionAlternate;
        holdInteractableAlternate.OnHoldInteractionAlternateEnd -= HoldInteractableAlternate_OnHoldInteractionAlternateEnd;

        holdInteractableAlternate.OnUpdatedInteractableAlternateState -= HoldInteractableAlternate_OnUpdatedInteractableAlternateState;
    }

    private void Awake()
    {
        InitializeComponents();

        HideSelectionUI();
        HideHoldUI();
    }

    private void Start()
    {
        SetInteractableAlternateSelectionText();
        SetInteractableAlternateHoldText();

        GeneralUIMethods.SetImageFillRatio(indicatorImage, 0f);
    }

    private void InitializeComponents()
    {
        holdInteractableAlternate = holdInteractableAlternateComponent.GetComponent<IHoldInteractableAlternate>();
        if (holdInteractableAlternate == null) Debug.LogError("The holdInteractableAlternate component does not implement IHoldInteractableAlternate");
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

    private void SetInteractableAlternateSelectionText() => interactableAlternateSelectionText.text = $"{holdInteractableAlternate.TooltipMessageAlternate}";
    private void SetInteractableAlternateHoldText() => interactableAlternateHoldText.text = $"{holdInteractableAlternate.TooltipMessageAlternate}";

    #region IHoldInteractableAlternate Event Subscriptions
    private void HoldInteractableAlternate_OnObjectSelected(object sender, System.EventArgs e)
    {
        SetInteractableAlternateSelectionText();
        SetInteractableAlternateHoldText();

        ShowSelectionUI();
    }

    private void HoldInteractableAlternate_OnObjectDeselected(object sender, System.EventArgs e)
    {
        HideSelectionUI();
        HideHoldUI();
    }

    private void HoldInteractableAlternate_OnHoldInteractionAlternateStart(object sender, System.EventArgs e)
    {
        HideSelectionUI();
        ShowHoldUI();
    }

    private void HoldInteractableAlternate_OnContinousHoldInteractionAlternate(object sender, IHoldInteractableAlternate.OnHoldInteractionAlternateEventArgs e)
    {
        GeneralUIMethods.SetImageFillRatio(indicatorImage, e.holdTimer / e.holdDuration);
    }

    private void HoldInteractableAlternate_OnHoldInteractionAlternateEnd(object sender, System.EventArgs e)
    {
        HideHoldUI();
        if(holdInteractableAlternate.IsSelectableAlternate) ShowSelectionUI();

        SetInteractableAlternateSelectionText();
        SetInteractableAlternateHoldText();
    }

    private void HoldInteractableAlternate_OnUpdatedInteractableAlternateState(object sender, System.EventArgs e)
    {
        SetInteractableAlternateSelectionText();
        SetInteractableAlternateHoldText();
    }

    #endregion
}
