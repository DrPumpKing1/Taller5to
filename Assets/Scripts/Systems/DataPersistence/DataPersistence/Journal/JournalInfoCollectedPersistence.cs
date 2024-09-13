using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalInfoCollectedPersistence : MonoBehaviour, IDataPersistence<JournalData>
{
    public void LoadData(JournalData data)
    {
        JournalInfoManager journalInfoManager = FindObjectOfType<JournalInfoManager>();

        foreach (JournalInfo journalInfo in data.journalInfoList)
        {
            if (journalInfo.isCollected)
            {
                journalInfoManager.AddJournalInfoToJournalByID(journalInfo.id);

                if(journalInfo.isChecked) journalInfoManager.CheckJournalInfoByID(journalInfo.id);

            }

        }
    }

    public void SaveData(ref JournalData data)
    {
        JournalInfoManager journalInfoManager = FindObjectOfType<JournalInfoManager>();

        foreach (JournalInfoManager.JournalInfoCheck journalInfoCheck in journalInfoManager.JournalInfoCollectedChecked) //Clear all data in data
        {
            foreach(JournalInfo journalInfo in data.journalInfoList)
            {
                if (journalInfoCheck.journalInfoSO.id == journalInfo.id) data.journalInfoList.Remove(journalInfo);
            }
        }

        foreach (JournalInfoManager.JournalInfoLog journalInfoLog in journalInfoManager.CompleteJournalInfoLogPool) 
        {
            bool isCollected = journalInfoManager.CheckIfJournalContainsJournalInfoCheck(journalInfoLog.journalInfoSO);
            bool isChecked = journalInfoManager.CheckIfJournalInfoIsChecked(journalInfoLog.journalInfoSO);

            JournalInfo journalInfo = new JournalInfo(journalInfoLog.journalInfoSO.id,isCollected,isChecked);

            data.journalInfoList.Add(journalInfo);          
        }
    }
}
