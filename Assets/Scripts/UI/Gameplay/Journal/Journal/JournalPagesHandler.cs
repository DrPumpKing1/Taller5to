using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class JournalPagesHandler : MonoBehaviour
{
    public static JournalPagesHandler Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private List<JournalPageButton> journalPageButtons;
    [SerializeField] private JournalPageButton currentJournalPageButton;
    [SerializeField] private JournalInfoPopUpUI currentJournalInfoPopUpUI;
    [Space]
    [SerializeField] private bool journalPagesHierarchyOverPopUps;
    [SerializeField, Range(0f, 1f)] private float buttonsCooldown;

    public bool JournalPagesHierarchyOverPopUps => journalPagesHierarchyOverPopUps;

    public static event EventHandler<OnJournalPageEventArgs> OnJournalPageOpen;
    public static event EventHandler<OnJournalPageEventArgs> OnJournalPageClose;

    private float cooldownTimer;

    private const int FIRST_PAGE_NUMBER = 1;

    [Serializable]
    public class JournalPageButton
    {
        public int pageNumber;
        public Button pageButton;
        public JournalPageUI journalPageUI;
    }

    public class OnJournalPageEventArgs : EventArgs
    {
        public JournalPageButton journalPageButton;
    }

    private void OnEnable()
    {
        JournalInfoPopUpUI.OnJournalInfoPopUpOpen += JournalInfoPopUpUI_OnJournalInfoPopUpOpen;
        JournalInfoPopUpUI.OnJournalInfoPopUpClose += JournalInfoPopUpUI_OnJournalInfoPopUpClose;
    }
    private void OnDisable()
    {
        JournalInfoPopUpUI.OnJournalInfoPopUpOpen -= JournalInfoPopUpUI_OnJournalInfoPopUpOpen;
        JournalInfoPopUpUI.OnJournalInfoPopUpClose -= JournalInfoPopUpUI_OnJournalInfoPopUpClose;
    }

    private void Awake()
    {
        SetSingleton();
        InitializeButtonsListeners();
    }

    private void Start()
    {
        ClearCurrentJournalInfoPopUpUI();
        HideAllPagesInmediately();
        ShowJournalPageInmediatelyByPageNumber(FIRST_PAGE_NUMBER);
        ResetCooldownTimer();
    }

    private void Update()
    {
        HandleButtonCooldown();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one JournalPagesHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeButtonsListeners()
    {
        foreach(JournalPageButton journalPageButton in journalPageButtons)
        {
            journalPageButton.pageButton.onClick.AddListener(() => OnJournalPageButtonClick(journalPageButton));
        }
    }

    private void HandleButtonCooldown()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
    }

    private void OnJournalPageButtonClick(JournalPageButton journalPageButton)
    {
        if (ButtonsOnCooldown()) return;
        if (currentJournalPageButton == journalPageButton && !currentJournalInfoPopUpUI) return; //If PageAlreadyOpen & JournalPopUp Not Opened
        //if (JournalInfoPopUpOpen && !journalPagesHierarchyOverPopUps) return;

        if (currentJournalPageButton == journalPageButton && currentJournalInfoPopUpUI)
        {
            currentJournalInfoPopUpUI.CloseFromPhysicalButtonClick();
            SetButtonsOnCooldown();
            return;
        }

        if(currentJournalPageButton != journalPageButton && !currentJournalInfoPopUpUI) 
        {
            HideJournalPage(currentJournalPageButton, false);
            ShowJournalPage(journalPageButton, false);
            SetButtonsOnCooldown();
            return;
        }

        if (currentJournalPageButton != journalPageButton && currentJournalInfoPopUpUI)
        {
            currentJournalInfoPopUpUI.CloseFromPhysicalButtonClick();
            HideJournalPage(currentJournalPageButton, true);
            ShowJournalPage(journalPageButton, true);
            SetButtonsOnCooldown();
            return;
        }
    } 

    private void ShowJournalPage(JournalPageButton journalPageButton, bool inmediately)
    {
        if(!inmediately) journalPageButton.journalPageUI.ShowPage();
        else journalPageButton.journalPageUI.ShowPageInmediately();


        journalPageButton.journalPageUI.transform.SetAsLastSibling();

        SetCurrentJournalPage(journalPageButton);

        OnJournalPageOpen?.Invoke(this, new OnJournalPageEventArgs { journalPageButton = journalPageButton });
    }

    private void HideJournalPage(JournalPageButton journalPageButton, bool inmediately)
    {
        if (!inmediately) journalPageButton.journalPageUI.HidePage();
        else journalPageButton.journalPageUI.HidePageInmediately();


        journalPageButton.journalPageUI.transform.SetAsFirstSibling();

        if (journalPageButton == currentJournalPageButton) ClearCurrentJournalPage();

        OnJournalPageClose?.Invoke(this, new OnJournalPageEventArgs { journalPageButton = journalPageButton });
    }

    private void ShowJournalPageInmediately(JournalPageButton journalPageButton)
    {
        journalPageButton.journalPageUI.ShowPageInmediately();
        journalPageButton.journalPageUI.transform.SetAsLastSibling();

        SetCurrentJournalPage(journalPageButton);
    }

    private void HideJournalPageInmediately(JournalPageButton journalPageButton)
    {
        journalPageButton.journalPageUI.HidePageInmediately();
        journalPageButton.journalPageUI.transform.SetAsFirstSibling();

        ClearCurrentJournalPage();
    }

    private void SetCurrentJournalPage(JournalPageButton journalPageButton) => currentJournalPageButton = journalPageButton;
    private void ClearCurrentJournalPage() => currentJournalPageButton = null;

    private void ResetCooldownTimer() => cooldownTimer = 0f;
    private void SetButtonsOnCooldown() => cooldownTimer = buttonsCooldown;
    private bool ButtonsOnCooldown() => cooldownTimer > 0f;

    private void ShowJournalPageInmediatelyByPageNumber(int pageNumber)
    {
        foreach(JournalPageButton journalPageButton in journalPageButtons)
        {
            if (journalPageButton.pageNumber == pageNumber)
            {
                ShowJournalPageInmediately(journalPageButton);
                return;
            }
        }
    }

    private void HideAllPagesInmediately()
    {
        foreach (JournalPageButton journalPageButton in journalPageButtons)
        {
            HideJournalPageInmediately(journalPageButton); 
        }
    }

    private void SetCurrentJournalInfoPopUpUI(JournalInfoPopUpUI journalInfoPopUpUI) => currentJournalInfoPopUpUI = journalInfoPopUpUI;
    private void ClearCurrentJournalInfoPopUpUI() => currentJournalInfoPopUpUI = null;

    #region JournalInfoPopUpUI Subscriptions
    private void JournalInfoPopUpUI_OnJournalInfoPopUpOpen(object sender, JournalInfoPopUpUI.OnJournalInfoPopUpEventArgs e)
    {
        SetCurrentJournalInfoPopUpUI(e.journalInfoPopUpUI);
    }
    private void JournalInfoPopUpUI_OnJournalInfoPopUpClose(object sender, JournalInfoPopUpUI.OnJournalInfoPopUpEventArgs e)
    {
        ClearCurrentJournalInfoPopUpUI();
    }

    #endregion
}
