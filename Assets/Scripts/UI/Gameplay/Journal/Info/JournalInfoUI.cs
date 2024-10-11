using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class JournalInfoUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private JournalInfoSO journalInfoSO;
    [SerializeField] private JournalInfoContentUI journalInfoContentUI;

    public event EventHandler OnJournalInfoHide;
    public event EventHandler OnJournalInfoCollected;
    public event EventHandler OnJournalInfoChecked;

    public JournalInfoSO JournalInfoSO => journalInfoSO;    

    private void OnEnable()
    {
        JournalInfoManager.OnJournalInfoCollected += JournalInfoManager_OnJournalInfoCollected;
        JournalInfoManager.OnJournalInfoChecked += JournalInfoManager_OnJournalInfoChecked;

        journalInfoContentUI.OnPopUpOpened += JournalInfoContentUI_OnPopUpOpened;
    }

    private void OnDisable()
    {
        JournalInfoManager.OnJournalInfoCollected -= JournalInfoManager_OnJournalInfoCollected;
        JournalInfoManager.OnJournalInfoChecked -= JournalInfoManager_OnJournalInfoChecked;

        journalInfoContentUI.OnPopUpOpened -= JournalInfoContentUI_OnPopUpOpened;
    }

    private void Start()
    {
        CheckJournalInfoState();
    }

    private void CheckJournalInfoState()
    {
        if (!JournalInfoManager.Instance.CheckIfJournalContainsJournalInfoCheck(journalInfoSO))
        {
            OnJournalInfoHide?.Invoke(this, EventArgs.Empty);
            return;
        }

        OnJournalInfoCollected.Invoke(this, EventArgs.Empty);

        if (JournalInfoManager.Instance.CheckIfJournalInfoIsChecked(journalInfoSO))
        {
            OnJournalInfoChecked.Invoke(this, EventArgs.Empty);
        }
    }

    private void JournalInfoManager_OnJournalInfoCollected(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        if (e.journalInfoSO != journalInfoSO) return;

        OnJournalInfoCollected?.Invoke(this, EventArgs.Empty);
    }

    private void JournalInfoManager_OnJournalInfoChecked(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        if (e.journalInfoSO != journalInfoSO) return;
        OnJournalInfoChecked?.Invoke(this, EventArgs.Empty);
    }

    private void JournalInfoContentUI_OnPopUpOpened(object sender, EventArgs e)
    {
        if (!JournalInfoManager.Instance.CheckIfJournalContainsJournalInfoCheck(journalInfoSO)) return; //JournalInfo not collected
        if (JournalInfoManager.Instance.CheckIfJournalInfoIsChecked(journalInfoSO)) return; //JournalInfo alreadyChecked

        JournalInfoManager.Instance.CheckJournalInfo(journalInfoSO);
    }
}
