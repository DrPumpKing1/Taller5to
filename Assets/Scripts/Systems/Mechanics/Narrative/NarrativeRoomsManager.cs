using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JournalInfoManager;
using static NarrativeRoomsManager;

public class NarrativeRoomsManager : MonoBehaviour
{
    public static NarrativeRoomsManager Instance { get; private set; }

    [Header("Journal Info Settings")]
    [SerializeField] private List<NarrativeRoomSO> narrativeRoomsVisited;
    [SerializeField] private List<NarrativeRoomLog> completeNarrativeRoomsLogPool;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<NarrativeRoomSO> NarrativeRoomsVisited => narrativeRoomsVisited;
    public List<NarrativeRoomLog> CompleteNarrativeRoomsLogPool => completeNarrativeRoomsLogPool;

    public static event EventHandler<OnNarrativeRoomEventArgs> OnNarrativeRoomVisited;

    [Serializable]
    public class NarrativeRoomLog
    {
        public NarrativeRoomSO narrativeRoomSO;
        public string logToVisit;
        [Range(0f,2f)] public float timeToVisit;
    }

    public class OnNarrativeRoomEventArgs : EventArgs
    {
        public NarrativeRoomSO narrativeRoomSO;
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
            Debug.LogWarning("There is more than one NarrativeRoomsManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Collection By Log
    private void CheckNarrativeRoomVisitByLog(string log)
    {
        foreach (NarrativeRoomLog narrativeRoomLog in completeNarrativeRoomsLogPool)
        {
            if (narrativeRoomLog.logToVisit == log) StartCoroutine(VisitNarrativeRoomCoroutine(narrativeRoomLog.narrativeRoomSO, narrativeRoomLog.timeToVisit));
        }
    }
    #endregion

    #region Addition To List

    private IEnumerator VisitNarrativeRoomCoroutine(NarrativeRoomSO narrativeRoomToVisit, float timeToVisit)
    {
        foreach (NarrativeRoomSO narrativeRoomSO in narrativeRoomsVisited)
        {
            if (narrativeRoomSO == narrativeRoomToVisit)
            {
                if (debug) Debug.Log($"Narrative room already visited, name: {narrativeRoomToVisit.roomName}");
                yield break;
            }
        }

        narrativeRoomsVisited.Add(narrativeRoomToVisit);

        yield return new WaitForSeconds(timeToVisit);

        OnNarrativeRoomVisited?.Invoke(this, new OnNarrativeRoomEventArgs { narrativeRoomSO = narrativeRoomToVisit });
    }

    public void VisitNarrativeRoom(NarrativeRoomSO narrativeRoomToVisit)
    {
        foreach (NarrativeRoomSO narrativeRoomSO in narrativeRoomsVisited)
        {
            if (narrativeRoomSO == narrativeRoomToVisit)
            {
                if (debug) Debug.Log($"Narrative room already visited, name: {narrativeRoomToVisit.roomName}");
                return;
            }
        }

        narrativeRoomsVisited.Add(narrativeRoomToVisit);

        OnNarrativeRoomVisited?.Invoke(this, new OnNarrativeRoomEventArgs { narrativeRoomSO = narrativeRoomToVisit });
    }

    public void AddNarrativeRoomToNarrativeRoomsVisitedByID(int id)
    {
        NarrativeRoomLog narrativeRoomLog = GetNarrativeRoomLogInCompletePoolByID(id);

        if (narrativeRoomLog == null)
        {
            if (debug) Debug.Log("Addition will be ignored due to narrative room info not found");
            return;
        }

        if (CheckIfNarrativeRoomsVisitedContainsNarrativeRoom(narrativeRoomLog.narrativeRoomSO))
        {
            if (debug) Debug.Log($"NarrativeRoomsVisited already contains narrative room with id: {narrativeRoomLog.narrativeRoomSO.id}");
            return;
        }

        narrativeRoomsVisited.Add(narrativeRoomLog.narrativeRoomSO);
    }

    public bool CheckIfNarrativeRoomsVisitedContainsNarrativeRoom(NarrativeRoomSO narrativeRoomSO)
    {
        foreach (NarrativeRoomSO narrativeRoomVisited in narrativeRoomsVisited)
        {
            if (narrativeRoomVisited == narrativeRoomSO) return true;
        }

        return false;
    }

    public bool CheckIfNarrativeRoomsVisitedContainsNarrativeRoomByID(int id)
    {
        foreach (NarrativeRoomSO narrativeRoomVisited in narrativeRoomsVisited)
        {
            if (narrativeRoomVisited.id == id) return true;
        }

        return false;
    }

    public NarrativeRoomLog GetNarrativeRoomLogInCompletePoolByID(int id)
    {
        foreach (NarrativeRoomLog narrativeRoomLog in completeNarrativeRoomsLogPool)
        {
            if (narrativeRoomLog.narrativeRoomSO.id == id) return narrativeRoomLog;
        }

        if (debug) Debug.Log($"Narrative room with id {id} not found in completePool");
        return null;
    }

    public NarrativeRoomSO GetNarrativeRoomInNarrativeRoomsVisitedByID(int id)
    {
        foreach (NarrativeRoomSO narrativeRoomVisited in narrativeRoomsVisited)
        {
            if (narrativeRoomVisited.id == id) return narrativeRoomVisited;
        }
        return null;
    }

    #endregion

    #region Count
    public int GetNarrativeRoomsCompletePoolQuantity() => completeNarrativeRoomsLogPool.Count;
    public int GetNarrativeRoomsVisitedQuantity() => narrativeRoomsVisited.Count;
    public bool HasVisitedAllNarrativeRooms() => GetNarrativeRoomsVisitedQuantity() >= GetNarrativeRoomsCompletePoolQuantity();
    #endregion

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckNarrativeRoomVisitByLog(e.gameplayAction.log);
    }
    #endregion
}
