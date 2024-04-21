using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoldInteractableAlternateSelectionUI : MonoBehaviour
{
    [Header("Hold Interactable Alternate Components")]
    [SerializeField] private Component holdInteractableAlternateComponent;

    [Header("UI Components")]
    [SerializeField] private CanvasGroup alternateSelectionUICanvasGroup;
    [SerializeField] private TextMeshProUGUI interactableAlternateSelectionText;
    [Space]
    [SerializeField] private CanvasGroup holdAlternateUICanvasGroup;
    [SerializeField] private TextMeshProUGUI interactableAlternateHoldText;
    [SerializeField] private Image indicatorImage;

    private IHoldInteractableAlternate holdInteractableAlternate;

    private void OnEnable()
    {
        holdInteractableAlternate.OnObjectSelectedAlternate += HoldInteractableAlternate_OnObjectSelected;
        holdInteractableAlternate.OnObjectDeselectedAlternate += HoldInteractableAlternate_OnObjectDeselected;

        holdInteractableAlternate.OnHoldInteractionAlternateStart += HoldInteractableAlternate_OnHoldInteractionAlternateStart; ;
        holdInteractableAlternate.OnContinousHoldInteractionAlternate += HoldInteractableAlternate_OnContinousHoldInteractionAlternate;
        holdInteractableAlternate.OnHoldInteractionAlternateEnd += HoldInteractableAlternate_OnHoldInteractionAlternateEnd; 
    }

    private void OnDisable()
    {
        holdInteractableAlternate.OnObjectSelectedAlternate -= HoldInteractableAlternate_OnObjectSelected;
        holdInteractableAlternate.OnObjectDeselectedAlternate -= HoldInteractableAlternate_OnObjectDeselected;

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

    private void HideSelectionUI()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(alternateSelectionUICanvasGroup, 0f);
    }

    private void ShowSelectionUI()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(alternateSelectionUICanvasGroup, 1f);
    }

    private void HideHoldUI()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(holdAlternateUICanvasGroup, 0f);
    }

    private void ShowHoldUI()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(holdAlternateUICanvasGroup, 1f);
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
        ShowSelectionUI();

        SetInteractableAlternateSelectionText();
        SetInteractableAlternateHoldText();
    }

    #endregion
}
