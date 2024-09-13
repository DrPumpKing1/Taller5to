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

    public event EventHandler OnJournalInfoCollected;
    public event EventHandler OnJournalInfoChecked;

    private void OnEnable()
    {
        JournalInfoManager.OnJournalInfoCollected += JournalInfoManager_OnJournalInfoCollected;
        journalInfoContentUI.OnMouseEnterContent += JournalInfoContentUI_OnMouseEnterContent;
    }

    private void OnDisable()
    {
        JournalInfoManager.OnJournalInfoCollected -= JournalInfoManager_OnJournalInfoCollected;
        journalInfoContentUI.OnMouseEnterContent -= JournalInfoContentUI_OnMouseEnterContent;
    }

    private void JournalInfoManager_OnJournalInfoCollected(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        OnJournalInfoCollected?.Invoke(this, EventArgs.Empty);
    }
    private void JournalInfoContentUI_OnMouseEnterContent(object sender, EventArgs e)
    {
        if (!JournalInfoManager.Instance.CheckIfJournalContainsJournalInfoCheck(journalInfoSO)) return; //JournalInfo not collected
        if (JournalInfoManager.Instance.CheckIfJournalInfoIsChecked(journalInfoSO)) return; //JournalInfo alreadyChecked

        JournalInfoManager.Instance.CheckJournalInfo(journalInfoSO);
        OnJournalInfoChecked?.Invoke(this, EventArgs.Empty);
    }
}
