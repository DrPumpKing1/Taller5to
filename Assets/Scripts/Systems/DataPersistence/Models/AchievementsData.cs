using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementsData
{
    public SerializableDictionary<int, bool> achievementsAchieved;

    public AchievementsData()
    {
        achievementsAchieved = new SerializableDictionary<int, bool>(); //string -> id; bool -> achieved;
    }
}
