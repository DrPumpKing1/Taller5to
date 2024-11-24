using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class JournalInfoContentUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler    
{
    [Header("Components")]
    [SerializeField] private JournalInfoUI journalInfoUI;
    [SerializeField] private GameObject contentGameObject;
    [SerializeField] private GameObject notCollectedGameObject;
    [SerializeField] private GameObject notCheckedIndicator;
    [Space]
    [SerializeField] private Button popUpOpeningButton;
    [Space]
    [SerializeField] private JournalInfoPopUpUI journalInfoPopUpUI;

    public event EventHandler OnMouseEnterContent;
    public event EventHandler OnMouseExitContent;
    public event EventHandler OnPopUpOpened;

    private void OnEnable()
    {
        journalInfoUI.OnJournalInfoHide += JournalInfoUI_OnJournalInfoHide;
        journalInfoUI.OnJournalInfoCollected += JournalInfoUI_OnJournalInfoCollected;
        journalInfoUI.OnJournalInfoChecked += JournalInfoUI_OnJournalInfoChecked;
    }

    private void OnDisable()
    {
        journalInfoUI.OnJournalInfoHide -= JournalInfoUI_OnJournalInfoHide;
        journalInfoUI.OnJournalInfoCollected -= JournalInfoUI_OnJournalInfoCollected;
        journalInfoUI.OnJournalInfoChecked -= JournalInfoUI_OnJournalInfoChecked;
    }

    private void Awake()
    {
        AddButtonsListeners();
    }

    private void AddButtonsListeners()
    {
        if (!popUpOpeningButton) return;
        popUpOpeningButton.onClick.AddListener(OpenPopUpUI);
    }

    private void HideJournalInfo()
    {
        contentGameObject.SetActive(false);
        notCollectedGameObject.SetActive(true);
        notCheckedIndicator.SetActive(false);
    }

    private void CollectJournalInfo()
    {
        contentGameObject.SetActive(true);
        notCollectedGameObject.SetActive(false);
        notCheckedIndicator.SetActive(true);
    }

    private void CheckJournalInfo()
    {
        notCheckedIndicator.SetActive(false);
    }

    private void OpenPopUpUI()
    {
        if (!journalInfoPopUpUI) return;

        journalInfoPopUpUI.OpenUIFromButton();
        OnPopUpOpened?.Invoke(this, EventArgs.Empty);
    }

    #region Pointer Methods
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterContent?.Invoke(this, EventArgs.Empty);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitContent?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region JournalInfoUISubscriptions
    private void JournalInfoUI_OnJournalInfoHide(object sender, EventArgs e)
    {
        HideJournalInfo();
    }

    private void JournalInfoUI_OnJournalInfoCollected(object sender, EventArgs e)
    {
        CollectJournalInfo();
    }

    private void JournalInfoUI_OnJournalInfoChecked(object sender, EventArgs e)
    {
        CheckJournalInfo();
    }
    #endregion
}
