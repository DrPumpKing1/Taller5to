using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsTriggeredPersistence : MonoBehaviour, IDataPersistence<LogData>
{
    public void LoadData(LogData data)
    {
        CinematicsManager cinematicsManager = FindObjectOfType<CinematicsManager>();

        foreach (KeyValuePair<int, bool> cinematicTriggered in data.cinematicsTriggered)
        {
            cinematicsManager.SetCinematicTriggered(cinematicTriggered.Key, cinematicTriggered.Value);
        }
    }

    public void SaveData(ref LogData data)
    {
        CinematicsManager cinematicsManager = FindObjectOfType<CinematicsManager>();

        for (int i = 0; i < cinematicsManager.Cinematics.Count; i++)
        {
            if (data.cinematicsTriggered.ContainsKey(i)) data.cinematicsTriggered.Remove(i);
        }

        for (int i = 0; i < cinematicsManager.Cinematics.Count; i++)
        {
            data.cinematicsTriggered.Add(i, cinematicsManager.Cinematics[i].triggered);
        }
    }
}