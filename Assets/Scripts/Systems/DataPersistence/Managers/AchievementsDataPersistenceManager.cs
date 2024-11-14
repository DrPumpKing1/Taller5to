using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsDataPersistenceManager : DataPersistenceManager<AchievementsData>
{
    public static AchievementsDataPersistenceManager Instance { get; private set; }

    private void OnEnable()
    {
        AchievementsManager.OnAchievementAchieved += AchievementsManager_OnAchievementAchieved;
    }
    private void OnDisable()
    {
        AchievementsManager.OnAchievementAchieved -= AchievementsManager_OnAchievementAchieved;
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

    private void AchievementsManager_OnAchievementAchieved(object sender, AchievementsManager.OnAchievementAchievedEventArgs e)
    {
        SaveGameData();
    }
    #endregion
}
