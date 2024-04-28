using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DictionarySelectionUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    [Header("UI Components")]
    [SerializeField] private List<DictionaryButtonPanel> dictionaryButtonPanels;

    private CanvasGroup canvasGroup;

    [Serializable]
    private class DictionaryButtonPanel
    {
        public Button dictionaryButton;
        public Transform dictionaryPanel;
    }

    private bool DictionaryInput => UIInput.GetDictionaryDown();

    public static event EventHandler OnDictionarySelectionUIOpen;
    public static event EventHandler OnDictionarySelectionUIClose;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeVariables();
        SetUIState(State.Closed);
    }

    private void Update()
    {
        CheckOpenClose();
    }

    private void InitializeButtonsListeners()
    {
        foreach(DictionaryButtonPanel dictionaryButtonPanel in dictionaryButtonPanels)
        {
            dictionaryButtonPanel.dictionaryButton.onClick.AddListener(() => OpenPanel(dictionaryButtonPanel.dictionaryPanel));
        }
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OpenPanel(Transform panel)
    {
        
    }

    private void CheckOpenClose()
    {
        bool openThisFrame = CheckOpen();

        if (openThisFrame) return;

        CheckClose();
    }

    private bool CheckOpen()
    {
        if (!DictionaryInput) return false;
        if (state != State.Closed) return false;

        OpenUI();

        return true;
    }

    private bool CheckClose()
    {
        if (!DictionaryInput) return false;
        if (state != State.Open) return false;

        CloseUI();

        return true;
    }

    private void OpenUI()
    {
        SetUIState(State.Open);

        AddToUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    protected override void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
