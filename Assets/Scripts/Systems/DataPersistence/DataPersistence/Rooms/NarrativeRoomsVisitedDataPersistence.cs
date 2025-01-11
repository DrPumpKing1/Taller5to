using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeRoomsVisitedDataPersistence : MonoBehaviour, IDataPersistence<RoomsData>
{
    public void LoadData(RoomsData data)
    {
        NarrativeRoomsManager narrativeRoomsManager = FindObjectOfType<NarrativeRoomsManager>();

        foreach (KeyValuePair<int, bool> narrativeRoomVisited in data.narrativeRoomsVisited)
        {
            if (narrativeRoomVisited.Value)
            {
                narrativeRoomsManager.AddNarrativeRoomToNarrativeRoomsVisitedByID(narrativeRoomVisited.Key);
            }
        }
    }

    public void SaveData(ref RoomsData data)
    {
        NarrativeRoomsManager narrativeRoomsManager = FindObjectOfType<NarrativeRoomsManager>();

        data.narrativeRoomsVisited.Clear();

        foreach (NarrativeRoomsManager.NarrativeRoomLog narrativeRoomLog in narrativeRoomsManager.CompleteNarrativeRoomsLogPool)
        {
            bool isVisited = narrativeRoomsManager.CheckIfNarrativeRoomsVisitedContainsNarrativeRoom(narrativeRoomLog.narrativeRoomSO);

            data.narrativeRoomsVisited.Add(narrativeRoomLog.narrativeRoomSO.id, isVisited);
        }
    }
}