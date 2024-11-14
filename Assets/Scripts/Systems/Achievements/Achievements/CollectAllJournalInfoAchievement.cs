using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAllJournalInfoAchievement : Achievement
{
    private void OnEnable()
    {
        JournalInfoManager.OnJournalInfoCollected += JournalInfoManager_OnJournalInfoCollected;
    }

    private void OnDisable()
    {
        JournalInfoManager.OnJournalInfoCollected -= JournalInfoManager_OnJournalInfoCollected;
    }

    protected override bool CheckCondition()
    {
        if (JournalInfoManager.Instance.HasCollectedAllJournalInfo()) return true;
        return false;
    }

    private void JournalInfoManager_OnJournalInfoCollected(object sender, JournalInfoManager.OnJournalInfoEventArgs e)
    {
        TryAchieve();
    }
}
