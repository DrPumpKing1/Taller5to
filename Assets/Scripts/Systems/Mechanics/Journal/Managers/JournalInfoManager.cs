using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static JournalInfoManager;

public class JournalInfoManager : MonoBehaviour
{
    public static JournalInfoManager Instance { get; private set; }

    [Header("Journal Info Settings")]
    [SerializeField] private List<JournalInfoCheck> journalInfoCollectedChecked;
    [SerializeField] private List<JournalInfoLog> completeJournalInfoLogPool;

    [Header("Debug")]
    [SerializeField] private bool debug;
    public List<JournalInfoCheck> JournalInfoCollectedChecked => journalInfoCollectedChecked;
    public List<JournalInfoLog> CompleteJournalInfoLogPool => completeJournalInfoLogPool;

    public static event EventHandler<OnJournalInfoEventArgs> OnJournalInfoCollected;
    public static event EventHandler<OnJournalInfoEventArgs> OnJournalInfoChecked;

    [Serializable]
    public class JournalInfoCheck
    {
        public JournalInfoSO journalInfoSO;
        public bool hasBeenChecked;
    }

    [Serializable]
    public class JournalInfoLog
    {
        public JournalInfoSO journalInfoSO;
        public string logToCollect;
    }

    public class OnJournalInfoEventArgs : EventArgs
    {
        public JournalInfoSO journalInfoSO;
    }

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }
    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
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

    #region Collection By Log
    private void CheckJournalInfoCollectionByLog(string log)
    {
        foreach (JournalInfoLog journalInfoLog in completeJournalInfoLogPool)
        {
            if (journalInfoLog.logToCollect == log) CollectJournalInfo(journalInfoLog.journalInfoSO);
        }
    }
    #endregion

    #region Addition To List
    public void CollectJournalInfo(JournalInfoSO journalInfoToCollect)
    {
        foreach(JournalInfoCheck journalInfoCheck in journalInfoCollectedChecked)
        {
            if(journalInfoCheck.journalInfoSO == journalInfoToCollect)
            {
                if (debug) Debug.Log($"Journal already contains info with name: {journalInfoToCollect.infoName}");
                return;
            }
        }

        JournalInfoCheck journalInfoCheckToAdd = new JournalInfoCheck { journalInfoSO = journalInfoToCollect, hasBeenChecked = false};
        journalInfoCollectedChecked.Add(journalInfoCheckToAdd);

        OnJournalInfoCollected?.Invoke(this, new OnJournalInfoEventArgs { journalInfoSO = journalInfoToCollect});
    }

    public void AddJournalInfoToJournalByID(int id)
    {
        JournalInfoLog journalInfoLog = GetJournalInfoLogInCompletePoolByID(id);

        if (journalInfoLog == null)
        {
            if (debug) Debug.LogWarning("Addition will be ignored due to journal info not found");
            return;
        }

        if (CheckIfJournalContainsJournalInfoCheck(journalInfoLog.journalInfoSO))
        {
            if (debug) Debug.Log($"Journal already contains journal info with id: {journalInfoLog.journalInfoSO.id}");
            return;
        }

        JournalInfoCheck journalInfoCheck = new JournalInfoCheck { journalInfoSO = journalInfoLog.journalInfoSO, hasBeenChecked = false };
        journalInfoCollectedChecked.Add(journalInfoCheck);
    }

    public bool CheckIfJournalContainsJournalInfoCheck(JournalInfoSO journalInfo)
    {
        foreach(JournalInfoCheck journalInfoCheck in journalInfoCollectedChecked)
        {
            if (journalInfoCheck.journalInfoSO == journalInfo) return true;
        }

        return false;
    }

    public bool CheckIfJournalContainsJournalInfoCheckByID(int id)
    {
        foreach (JournalInfoCheck jouralInfoCheck in journalInfoCollectedChecked)
        {
            if (jouralInfoCheck.journalInfoSO.id == id) return true;
        }

        return false;
    }

    public JournalInfoLog GetJournalInfoLogInCompletePoolByID(int id)
    {
        foreach (JournalInfoLog journalInfoLog in completeJournalInfoLogPool)
        {
            if (journalInfoLog.journalInfoSO.id == id) return journalInfoLog;
        }

        if (debug) Debug.LogWarning($"Journal Info with id {id} not found in completePool");
        return null;
    }

    public JournalInfoCheck GetJournalInfoCheckInJournalByID(int id)
    {
        foreach (JournalInfoCheck journalInfoCheck in journalInfoCollectedChecked)
        {
            if (journalInfoCheck.journalInfoSO.id == id) return journalInfoCheck;
        }
        return null;
    }
    #endregion

    #region Check Journal Info
    public void CheckJournalInfo(JournalInfoSO journalInfoToCheck)
    {
        foreach(JournalInfoCheck journalInfoCheck in journalInfoCollectedChecked)
        {
            if (journalInfoCheck.journalInfoSO == journalInfoToCheck)
            {
                if (journalInfoCheck.hasBeenChecked)
                {
                    if (debug) Debug.LogWarning($"Journal Info with id {journalInfoToCheck.id} has alreadyBeenChecked");
                }
                else
                {
                    journalInfoCheck.hasBeenChecked = true;
                    OnJournalInfoChecked?.Invoke(this, new OnJournalInfoEventArgs { journalInfoSO = journalInfoToCheck });
                }

                return;
            }
        }

        if (debug) Debug.LogWarning($"Journal Info with id {journalInfoToCheck.id} not found in collectedJournalInfo");
    }

    public void CheckJournalInfoByID(int id)
    {
        foreach (JournalInfoCheck journalInfoCheck in journalInfoCollectedChecked)
        {
            if (journalInfoCheck.journalInfoSO.id == id)
            {
                journalInfoCheck.hasBeenChecked = true;
                return;
            }
        }
    }

    public bool CheckIfJournalInfoIsChecked(JournalInfoSO journalInfoSO)
    {
        foreach(JournalInfoCheck journalInfoCheck in journalInfoCollectedChecked)
        {
            if(journalInfoCheck.journalInfoSO == journalInfoSO)
            {
                if (journalInfoCheck.hasBeenChecked) return true;
                else return false;
            }
        }

        return false;
    }
    #endregion

    public void ReplaceJournalInfoCollectedList(List<JournalInfoSO> journalInfosSOs)
    {
        journalInfoCollectedChecked.Clear();

        foreach (JournalInfoSO journalInfoSO in journalInfosSOs)
        {
            if (CheckIfJournalContainsJournalInfoCheck(journalInfoSO)) continue;

            JournalInfoCheck journalInfoCheckToAdd = new JournalInfoCheck { journalInfoSO = journalInfoSO, hasBeenChecked = true };
            journalInfoCollectedChecked.Add(journalInfoCheckToAdd);
        }
    }

    #region GameLogManagerSubscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckJournalInfoCollectionByLog(e.gameplayAction.log);
    }
    #endregion
}
