using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsDataPersistence : MonoBehaviour, IDataPersistence<AchievementsData>
{
    public void LoadData(AchievementsData data)
    {
        Achievement[] achievements = FindObjectsOfType<Achievement>();

        foreach (Achievement achievement in achievements)
        {
            foreach (KeyValuePair<int, bool> achievementAchieved in data.achievementsAchieved)
            {
                if (achievement.ID == 0) continue;

                if (achievement.ID == achievementAchieved.Key)
                {
                    if (achievementAchieved.Value) achievement.SetHasBeenAchieved(true);
                    else achievement.SetHasBeenAchieved(false);
                    break;
                }
            }
        }
    }

    public void SaveData(ref AchievementsData data)
    {
        Achievement[] achievements = FindObjectsOfType<Achievement>();

        foreach (Achievement achievement in achievements)
        {
            if (data.achievementsAchieved.ContainsKey(achievement.ID)) data.achievementsAchieved.Remove(achievement.ID);
        }

        foreach (Achievement achievement in achievements)
        {
            if (achievement.ID == 0) continue;

            bool achieved = achievement.IsAchieved;
            data.achievementsAchieved.Add(achievement.ID, achieved);
        }
    }
}