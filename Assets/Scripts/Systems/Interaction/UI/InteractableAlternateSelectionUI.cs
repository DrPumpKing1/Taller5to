using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableAlternateSelectionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Component interactableAlternateComponent;
    [SerializeField] private TextMeshProUGUI interactableAlternateSelectionText;

    private IInteractableAlternate interactableAlternate;
    private CanvasGroup canvasGroup;

    private const string INTERACT_ALTERNATE_KEY = "[R]";

    private void OnEnable()
    {
        interactableAlternate.OnObjectSelectedAlternate += InteractableAlternate_OnObjectSelectedAlternate;
        interactableAlternate.OnObjectDeselectedAlternate += InteractableAlternate_OnObjectDeselectedAlternate;
    }

    private void OnDisable()
    {
        interactableAlternate.OnObjectSelectedAlternate -= InteractableAlternate_OnObjectSelectedAlternate;
        interactableAlternate.OnObjectDeselectedAlternate -= InteractableAlternate_OnObjectDeselectedAlternate;
    }

    private void Awake()
    {
        InitializeComponents();
        HideUI();
    }

    private void Start()
    {
        SetInteractableAlternateSelectionText();
    }

    private void InitializeComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        interactableAlternate = interactableAlternateComponent.GetComponent<IInteractableAlternate>();
        if (interactableAlternate == null) Debug.LogError("The interactableAlternate component does not implement IInteractableAlternate");
    }

    private void HideUI()
    {
        SetCanvasGroupAlpha(0f);
    }

    private void ShowUI()
    {
        SetCanvasGroupAlpha(1f);
    }

    private void SetInteractableAlternateSelectionText() => interactableAlternateSelectionText.text = $"{INTERACT_ALTERNATE_KEY} {interactableAlternate.TooltipMessageAlternate}";
    private void SetCanvasGroupAlpha(float alpha) => canvasGroup.alpha = alpha;

    #region IInteractableAlternate Event Subscriptions
    private void InteractableAlternate_OnObjectSelectedAlternate(object sender, System.EventArgs e)
    {
        ShowUI();
    }

    private void InteractableAlternate_OnObjectDeselectedAlternate(object sender, System.EventArgs e)
    {
        HideUI();
    }
    #endregion
}
