using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DiegeticInscriptionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InscriptionRead inscriptionTranslation;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI inscriptionTitle;
    [SerializeField] private TextMeshProUGUI inscriptionText;

    public static event EventHandler OnAnyInscriptionTranslationUIOpen;
    public static event EventHandler OnAnyInscriptionTranslationUIClose;

    private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        inscriptionTranslation.OnOpenTranslationUI += InscriptionTranslation_OnOpenTranslationUI;
        inscriptionTranslation.OnCloseTranslationUI += InscriptionTranslation_OnCloseTranslationUI;
    }

    private void OnDisable()
    {
        inscriptionTranslation.OnOpenTranslationUI -= InscriptionTranslation_OnOpenTranslationUI;
        inscriptionTranslation.OnCloseTranslationUI -= InscriptionTranslation_OnCloseTranslationUI;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        SetInscriptionTitle();
        SetInscriptionText();

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void SetInscriptionTitle() => inscriptionTitle.text = inscriptionTranslation.Inscription.InscriptionSO.title;
    private void SetInscriptionText() => inscriptionText.text = inscriptionTranslation.Inscription.InscriptionSO.text;

    private void OpenDiegeticUI()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        OnAnyInscriptionTranslationUIOpen?.Invoke(this, EventArgs.Empty);

    }

    private void CloseDiegeticUI()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        OnAnyInscriptionTranslationUIClose?.Invoke(this, EventArgs.Empty);
    }

    #region InscriptionTranslation Subscriptions
    private void InscriptionTranslation_OnOpenTranslationUI(object sender, EventArgs e)
    {
        OpenDiegeticUI();
    }

    private void InscriptionTranslation_OnCloseTranslationUI(object sender, EventArgs e)
    {
        CloseDiegeticUI();
    }

    #endregion
}
