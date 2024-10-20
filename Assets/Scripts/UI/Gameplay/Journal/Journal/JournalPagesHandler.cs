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
    [Space]
    [SerializeField] private bool journalPagesHierarchyOverPopUps;
    [SerializeField, Range(0f, 1f)] private float buttonsCooldown;

    public bool JournalInfoPopUpOpen { get; private set; }
    public bool JournalPagesHierarchyOverPopUps => journalPagesHierarchyOverPopUps;

    public static event EventHandler<OnJournalPageEventArgs> OnJournalPageOpen;
    public static event EventHandler<OnJournalPageEventArgs> OnJournalPageClose;

    public float cooldownTimer;

    [System.Serializable]
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
        InitializeVariables();
        HideAllPagesInmediately();
        ShowJournalPageInmediatelyByPageNumber(1);
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
        if (currentJournalPageButton == journalPageButton) return;
        if (JournalInfoPopUpOpen && !journalPagesHierarchyOverPopUps) return;
        if (ButtonsOnCooldown()) return;

        if(currentJournalPageButton != null) 
        {
            HideJournalPage(currentJournalPageButton);
        }

        ShowJournalPage(journalPageButton);
        SetButtonsOnCooldown();
    } 

    private void InitializeVariables()
    {
        JournalInfoPopUpOpen = false;
    }

    private void ShowJournalPage(JournalPageButton journalPageButton)
    {
        journalPageButton.journalPageUI.ShowPage();
        journalPageButton.journalPageUI.transform.SetAsLastSibling();

        SetCurrentJournalPage(journalPageButton);

        OnJournalPageOpen?.Invoke(this, new OnJournalPageEventArgs { journalPageButton = journalPageButton });
    }

    private void HideJournalPage(JournalPageButton journalPageButton)
    {
        journalPageButton.journalPageUI.HidePage();
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
