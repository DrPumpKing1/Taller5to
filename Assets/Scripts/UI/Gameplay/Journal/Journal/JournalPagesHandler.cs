using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalPagesHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<JournalPageButton> journalPageButtons;
    [SerializeField] private JournalPageButton currentJournalPageButton;

    public bool JournalInfoPopUpOpen { get; private set; }

    [System.Serializable]
    public class JournalPageButton
    {
        public int pageNumber;
        public Button pageButton;
        public JournalPageUI journalPageUI;
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
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeVariables();
        HideAllPagesInmediately();
        ShowJournalPageInmediatelyByPageNumber(1);
    }

    private void InitializeButtonsListeners()
    {
        foreach(JournalPageButton journalPageButton in journalPageButtons)
        {
            journalPageButton.pageButton.onClick.AddListener(() => OnJournalPageButtonClick(journalPageButton));
        }
    }

    private void OnJournalPageButtonClick(JournalPageButton journalPageButton)
    {
        if (currentJournalPageButton == journalPageButton) return;
        if (JournalInfoPopUpOpen) return;

        if(currentJournalPageButton != null) 
        {
            HideJournalPage(currentJournalPageButton);
        }

        ShowJournalPage(journalPageButton);
    }

    private void InitializeVariables()
    {
        JournalInfoPopUpOpen = false;
    }

    private void ShowJournalPage(JournalPageButton journalPageButton)
    {
        journalPageButton.journalPageUI.ShowPage();
        SetCurrentJournalPage(journalPageButton);
    }

    private void HideJournalPage(JournalPageButton journalPageButton)
    {
        journalPageButton.journalPageUI.HidePage();
        ClearCurrentJournalPage();
    }

    private void ShowJournalPageInmediately(JournalPageButton journalPageButton)
    {
        journalPageButton.journalPageUI.ShowPageInmediately();
        SetCurrentJournalPage(journalPageButton);
    }

    private void HideJournalPageInmediately(JournalPageButton journalPageButton)
    {
        journalPageButton.journalPageUI.HidePageInmediately();
        ClearCurrentJournalPage();
    }

    private void SetCurrentJournalPage(JournalPageButton journalPageButton) => currentJournalPageButton = journalPageButton;
    private void ClearCurrentJournalPage() => currentJournalPageButton = null;  

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

    #region JournalInfoPopUpUI Subscriptions
    private void JournalInfoPopUpUI_OnJournalInfoPopUpOpen(object sender, System.EventArgs e)
    {
        JournalInfoPopUpOpen = true;
    }
    private void JournalInfoPopUpUI_OnJournalInfoPopUpClose(object sender, System.EventArgs e)
    {
        JournalInfoPopUpOpen = false;
    }

    #endregion
}
