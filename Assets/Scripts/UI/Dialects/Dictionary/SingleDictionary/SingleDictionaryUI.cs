using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SingleDictionaryUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    [Header("UI Components")]
    [SerializeField] private Transform symbolsContainer;
    [SerializeField] private Button closeButton;

    [Header("Settings")]
    [SerializeField] private Dialect dialect;

    public static event EventHandler OnSingleDictionaryUIOpen;
    public static event EventHandler OnSingleDictionaryUIClose;

    public class OnSingleDiccionaryUIEventArgs : EventArgs
    {
        public Dialect dialect;
    }

    private bool DictionaryInput => UIInput.GetDictionaryDown();
    private CanvasGroup canvasGroup;

    protected override void OnEnable()
    {
        base.OnEnable();
        DictionaryOpeningUIHandler.OnOpenSingleDictionaryUI += DictionarySelectionUI_OnOpenSingleDictionaryUI;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DictionaryOpeningUIHandler.OnOpenSingleDictionaryUI -= DictionarySelectionUI_OnOpenSingleDictionaryUI;
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

    private void LateUpdate() //If put on Update, both this and DictionarySelectionUI Will close
    {
        //CheckClose();
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

    public void OpenUI()
    {
        if (state != State.Closed) return;

        SetUIState(State.Open);

        AddToUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        OnSingleDictionaryUIOpen?.Invoke(this, new OnSingleDiccionaryUIEventArgs { dialect = dialect });
    }

    public void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnSingleDictionaryUIClose?.Invoke(this, new OnSingleDiccionaryUIEventArgs { dialect = dialect });
    }

    protected override void CloseFromUI()
    {
        CloseUI();
    }

    private void CheckClose()
    {
        if (!UIManager.Instance.IsFirstOnList(this)) return;
        if (!DictionaryInput) return;
        if (state != State.Open) return;

        CloseFromUI();
    }

    #region DictionarySelectionUI Subscriptions
    private void DictionarySelectionUI_OnOpenSingleDictionaryUI(object sender, DictionaryOpeningUIHandler.OnOpenSingleDictionaryUIEventArgs e)
    {
        if (e.singleDictionaryUI != this) return;

        OpenUI();
    }
    #endregion
}
