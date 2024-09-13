using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalOpeningPersistence : MonoBehaviour, IDataPersistence<UIData>
{
    public void LoadData(UIData data)
    {
        JournalOpeningManager journalOpeningManager = FindObjectOfType<JournalOpeningManager>();

        if (!data.canOpenJournal) return; //If its false, it means UIData has been initialized as a new()

        if (data.canOpenJournal) journalOpeningManager.SetCanOpenJournal(true);
        else journalOpeningManager.SetCanOpenJournal(false);
    }

    public void SaveData(ref UIData data)
    {
        JournalOpeningManager journalOpeningManager = FindObjectOfType<JournalOpeningManager>();

        data.canOpenJournal = journalOpeningManager.CanOpenJournal;
    }
}

