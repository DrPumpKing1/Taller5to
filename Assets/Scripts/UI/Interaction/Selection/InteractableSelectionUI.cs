using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableSelectionUI : MonoBehaviour
{
    [Header("Interactable Components")]
    [SerializeField] private Component interactableComponent;

    [Header("UI Components")]
    [SerializeField] private CanvasGroup selectionUICanvasGroup;
    [SerializeField] private TextMeshProUGUI interactableSelectionText;

    private IInteractable interactable;

    private void OnEnable()
    {
        interactable.OnObjectSelected += Interactable_OnObjectSelected;
        interactable.OnObjectDeselected += Interactable_OnObjectDeselected;
        interactable.OnObjectInteracted += Interactable_OnObjectInteracted;
    }

    private void OnDisable()
    {
        interactable.OnObjectSelected -= Interactable_OnObjectSelected;
        interactable.OnObjectDeselected -= Interactable_OnObjectDeselected;
        interactable.OnObjectInteracted -= Interactable_OnObjectInteracted;
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
        GeneralUIMethods.SetCanvasGroupAlpha(selectionUICanvasGroup,0f);
    }

    public void ShowSelectionUI()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(selectionUICanvasGroup, 1f);
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
    #endregion
}
