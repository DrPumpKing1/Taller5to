using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableAlternateSelectionUI : MonoBehaviour
{
    [Header("Interactable Alternate Components")]
    [SerializeField] private Component interactableAlternateComponent;

    [Header("UI Components")]
    [SerializeField] private CanvasGroup alternateSelectionUICanvasGroup;
    [SerializeField] private TextMeshProUGUI interactableAlternateSelectionText;

    private IInteractableAlternate interactableAlternate;

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
        interactableAlternate.OnObjectInteractedAlternate += InteractableAlternate_OnObjectInteractedAlternate;
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
        GeneralUIMethods.SetCanvasGroupAlpha(alternateSelectionUICanvasGroup, 0f);
    }

    public void ShowSelectionUI()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(alternateSelectionUICanvasGroup, 1f);
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
