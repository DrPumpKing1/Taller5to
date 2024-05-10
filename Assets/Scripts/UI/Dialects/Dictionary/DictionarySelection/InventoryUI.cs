using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryUI : BaseUI
{
    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    private CanvasGroup canvasGroup;

    public static event EventHandler OnCloseFromUI;
    public static event EventHandler OnInventoryUIOpen;
    public static event EventHandler OnInventoryUIClose;

    protected override void OnEnable()
    {
        base.OnEnable();
        InventoryOpeningManager.OnInventoryOpen += DictionarySelectionOpeningManager_OnDictionarySelectionOpen;
        InventoryOpeningManager.OnInventoryClose += DictionarySelectionOpeningManager_OnDictionarySelectionClose;

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InventoryOpeningManager.OnInventoryOpen -= DictionarySelectionOpeningManager_OnDictionarySelectionOpen;
        InventoryOpeningManager.OnInventoryClose -= DictionarySelectionOpeningManager_OnDictionarySelectionClose;
    }

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

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseFromUI);
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        OnInventoryUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnInventoryUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        OnCloseFromUI?.Invoke(this, EventArgs.Empty);
    }


    #region DictionarySelectionOpeningManager Subscriptions
    private void DictionarySelectionOpeningManager_OnDictionarySelectionOpen(object sender, EventArgs e)
    {
        OpenUI();
    }

    private void DictionarySelectionOpeningManager_OnDictionarySelectionClose(object sender, EventArgs e)
    {
        CloseUI();
    }
    #endregion
}
