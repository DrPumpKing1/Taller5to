using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalInfoCollectionHandler : MonoBehaviour
{
    [Header("Journal Info-Log List")]
    [SerializeField] private List<JournalInfoSOLogRelation> journalInfoSOLogRelations = new List<JournalInfoSOLogRelation>();

    public class JournalInfoSOLogRelation
    {
        public JournalInfoSO journalInfoSO;
        public string logToCollect;
    }

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }
    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckJournalInfoCollectionByLog(string log)
    {
        foreach(JournalInfoSOLogRelation journalInfoSOLogRelation in journalInfoSOLogRelations)
        {
            if (journalInfoSOLogRelation.logToCollect == log) JournalInfoManager.Instance.CollectJournalInfo(journalInfoSOLogRelation.journalInfoSO);
        }
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckJournalInfoCollectionByLog(e.gameplayAction.log);
    }
}
