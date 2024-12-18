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
    [SerializeField, Range(0f, 1f)] private float buttonsCooldown;

    public static event EventHandler<OnJournalPageEventArgs> OnJournalPageOpen;
    public static event EventHandler<OnJournalPageEventArgs> OnJournalPageClose;

    public static event EventHandler OnJournalPageButtonEffectiveClick;

    private float cooldownTimer;

    private const int FIRST_PAGE_NUMBER = 1;

    private bool onPopUp;

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
        InitializeVariables();
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

    private void InitializeVariables()
    {
        onPopUp = true;
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

        if(currentJournalPageButton != journalPageButton) //Only if other page clicked
        {
            if (onPopUp) //If is onPopUp, do an inmediate transition. The popUp hiding will hide the inmediate change of the new journal page
            {
                HideJournalPage(currentJournalPageButton, true);
                ShowJournalPage(journalPageButton, true);    
            }
            else //Otherwise, do a normal(not inmediate) transition
            {
                HideJournalPage(currentJournalPageButton, false);
                ShowJournalPage(journalPageButton, false);
            }
        }

        OnJournalPageButtonEffectiveClick?.Invoke(this, EventArgs.Empty);

        SetButtonsOnCooldown();
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

    #region JournalInfoPopUpUI Subscriptions
    private void JournalInfoPopUpUI_OnJournalInfoPopUpOpen(object sender, JournalInfoPopUpUI.OnJournalInfoPopUpEventArgs e)
    {
        onPopUp = true;
    }
    private void JournalInfoPopUpUI_OnJournalInfoPopUpClose(object sender, JournalInfoPopUpUI.OnJournalInfoPopUpEventArgs e)
    {
        onPopUp = false;
    }

    #endregion
}
