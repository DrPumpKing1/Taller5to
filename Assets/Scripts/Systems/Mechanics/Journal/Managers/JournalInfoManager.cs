using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JournalInfoManager : MonoBehaviour
{
    public static JournalInfoManager Instance { get; private set; }

    [Header("Journal Info Settings")]
    [SerializeField] private List<JournalInfoSO> journalInfoCollected;
    [SerializeField] private List<JournalInfoSO> completeJournalInfoPool;

    [Header("Debug")]
    [SerializeField] private bool debug;
    public List<JournalInfoSO> JournalInfoCollected => journalInfoCollected;
    public List<JournalInfoSO> CompleteJournalInfoPool => completeJournalInfoPool;

    public static event EventHandler<OnJournalInfoCollectedEventArgs> OnJournalInfoCollected;

    public class OnJournalInfoCollectedEventArgs : EventArgs
    {
        public JournalInfoSO journalInfoSO;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one JournalInfoManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void CollectJournalInfo(JournalInfoSO journalInfoToCollect)
    {
        if (journalInfoCollected.Contains(journalInfoToCollect))
        {
            if (debug) Debug.Log($"Journal already contains info with name: {journalInfoToCollect.infoName}");
            return;
        }

        OnJournalInfoCollected?.Invoke(this, new OnJournalInfoCollectedEventArgs { journalInfoSO = journalInfoToCollect});
        journalInfoCollected.Add(journalInfoToCollect);
    }

    public void AddJournalInfoToJournalByID(int id)
    {
        JournalInfoSO journalInfoToCollect = GetJournalInfoInCompletePoolByID(id);

        if (!journalInfoToCollect)
        {
            if (debug) Debug.LogWarning("Addition will be ignored due to journal info piece not found");
            return;
        }

        if (CheckIfJournalContainsJournalInfo(journalInfoToCollect))
        {
            if (debug) Debug.Log($"Journal already contains journal info with id: {journalInfoToCollect.id}");
            return;
        }

        journalInfoCollected.Add(journalInfoToCollect);
    }

    public bool CheckIfJournalContainsJournalInfo(JournalInfoSO journalInfo) => journalInfoCollected.Contains(journalInfo);

    public bool CheckIfJournalContainsShieldPieceByID(int id)
    {
        foreach (JournalInfoSO journalInfo in journalInfoCollected)
        {
            if (journalInfo.id == id) return true;
        }

        return false;
    }

    public JournalInfoSO GetJournalInfoInCompletePoolByID(int id)
    {
        foreach (JournalInfoSO journalInfo in completeJournalInfoPool)
        {
            if (journalInfo.id == id) return journalInfo;
        }

        if (debug) Debug.LogWarning($"Journal Info with id {id} not found in completePool");
        return null;
    }

    public JournalInfoSO GetJournalInfoInJournalByID(int id)
    {
        foreach (JournalInfoSO journalInfo in journalInfoCollected)
        {
            if (journalInfo.id == id) return journalInfo;
        }
        return null;
    }

    public void ReplaceJournalInfoCollectedList(List<JournalInfoSO> journalInfosSOs)
    {
        journalInfoCollected.Clear();

        foreach (JournalInfoSO journalInfoSO in journalInfosSOs)
        {
            if (journalInfoCollected.Contains(journalInfoSO)) continue;
            journalInfoCollected.Add(journalInfoSO);
        }
    }
}
