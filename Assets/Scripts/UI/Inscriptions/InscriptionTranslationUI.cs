using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class InscriptionTranslationUI : BaseUI
{
    [Header("Components")]
    [SerializeField] private InscriptionRead inscriptionTranslation;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    public static event EventHandler OnAnyInscriptionTranslationUIOpen;
    public static event EventHandler OnAnyInscriptionTranslationUIClose;

    private CanvasGroup canvasGroup;

    protected override void OnEnable()
    {
        base.OnEnable();
        inscriptionTranslation.OnOpenTranslationUI += InscriptionTranslation_OnOpenTranslationUI;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inscriptionTranslation.OnOpenTranslationUI -= InscriptionTranslation_OnOpenTranslationUI;
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

        OnAnyInscriptionTranslationUIOpen?.Invoke(this, EventArgs.Empty);

    }

    private void CloseUI()
    {
        if (state != State.Open) return;

        SetUIState(State.Closed);

        RemoveFromUILayersList();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnAnyInscriptionTranslationUIClose?.Invoke(this, EventArgs.Empty);
    }

    protected override void CloseFromUI()
    {
        CloseUI();
    }

    #region InscriptionTranslation Subscriptions
    private void InscriptionTranslation_OnOpenTranslationUI(object sender, EventArgs e)
    {
        OpenUI();
    }
    #endregion
}