using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableSelectionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Component interactableComponent;
    [SerializeField] private TextMeshProUGUI interactableSelectionText;

    private IInteractable interactable;
    private CanvasGroup canvasGroup;

    private const string INTERACT_KEY = "[E]";

    private void OnEnable()
    {
        interactable.OnObjectSelected += Interactable_OnObjectSelected;
        interactable.OnObjectDeselected += Interactable_OnObjectDeselected;
    }

    private void OnDisable()
    {
        interactable.OnObjectSelected -= Interactable_OnObjectSelected;
        interactable.OnObjectDeselected -= Interactable_OnObjectDeselected;
    }

    private void Awake()
    {
        InitializeComponents();
        HideUI();
    }

    private void Start()
    {
        SetInteractableSelectionText();
    }

    private void InitializeComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        interactable = interactableComponent.GetComponent<IInteractable>();
        if (interactable == null) Debug.LogError("The interactable component does not implement IInteractable");
    }

    private void HideUI()
    {
        SetCanvasGroupAlpha(0f);
    }

    private void ShowUI()
    {
        SetCanvasGroupAlpha(1f);
    }

    private void SetInteractableSelectionText() => interactableSelectionText.text = $"{INTERACT_KEY} {interactable.TooltipMessage}";
    private void SetCanvasGroupAlpha(float alpha) => canvasGroup.alpha = alpha;

    #region IInteractable Event Subscriptions
    private void Interactable_OnObjectSelected(object sender, System.EventArgs e)
    {
        ShowUI();
    }

    private void Interactable_OnObjectDeselected(object sender, System.EventArgs e)
    {
        HideUI();
    }
    #endregion
}
