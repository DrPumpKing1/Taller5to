using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsDataPersistenceManager : DataPersistenceManager<AchievementsData>
{
    public static AchievementsDataPersistenceManager Instance { get; private set; }

    private void OnEnable()
    {
        AchievementManager.OnAchievementAchieved += AchievementManager_OnAchievementAchieved;
    }
    private void OnDisable()
    {
        AchievementManager.OnAchievementAchieved -= AchievementManager_OnAchievementAchieved;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AchievementsDataPersistenceManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region CheckpointManager Subscriptions

    private void AchievementManager_OnAchievementAchieved(object sender, AchievementManager.OnAchievementAchievedEventArgs e)
    {
        SaveGameData();
    }
    #endregion
}
