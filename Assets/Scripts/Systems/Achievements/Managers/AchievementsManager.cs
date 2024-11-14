using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debug;    

    public static event EventHandler<OnAchievementAchievedEventArgs> OnAchievementAchieved;

    public class OnAchievementAchievedEventArgs : EventArgs
    {
        public AchievementSO achievementSO;
    }

    private void OnEnable()
    {
        Achievement.OnAchievementAchieved += Achievement_OnAchievementAchieved;
    }

    private void OnDisable()
    {
        Achievement.OnAchievementAchieved -= Achievement_OnAchievementAchieved;
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
        }
        else
        {
            Debug.LogWarning("There is more than one AchievementManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void AchieveAchievement(AchievementSO achievementSO)
    {
        OnAchievementAchieved?.Invoke(this, new OnAchievementAchievedEventArgs { achievementSO = achievementSO });
        if(debug) Debug.Log($"Achievement with name {achievementSO.name} achieved!");
    }

    #region Achievement Subcriptions
    private void Achievement_OnAchievementAchieved(object sender, Achievement.OnAchievementAchievedEventArgs e)
    {
        AchieveAchievement(e.achievementSO);
    }
    #endregion
}
