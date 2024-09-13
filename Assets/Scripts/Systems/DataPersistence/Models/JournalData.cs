using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class JournalData
{
    public List<JournalInfo> journalInfoList;

    public JournalData()
    {
        journalInfoList = new List<JournalInfo>();
    }
}

[System.Serializable]
public class JournalInfo
{
    public int id;
    public bool isCollected;
    public bool isChecked;

    public JournalInfo(int id, bool isCollected, bool isChecked)
    {
        this.id = id;
        this.isCollected = isCollected;
        this.isChecked = isChecked;
    }   
}
