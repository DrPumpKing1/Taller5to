using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Achievement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AchievementSO achievementSO;

    [Header("Settings")]
    [SerializeField] private bool hasBeenAchieved;

    public int ID => achievementSO.id;
    public bool IsAchieved => hasBeenAchieved;

    public static event EventHandler<OnAchievementAchievedEventArgs> OnAchievementAchieved;

    public class OnAchievementAchievedEventArgs : EventArgs
    {
        public AchievementSO achievementSO;
    }

    public void SetHasBeenAchieved(bool achieved) => hasBeenAchieved = achieved;
    protected bool AlreadyAchieved() => hasBeenAchieved;
    protected abstract bool CheckCondition();

    protected void TryAchieve()
    {
        if (AlreadyAchieved()) return;
        if (!CheckCondition()) return;

        AchieveAchievement();
    }

    protected void AchieveAchievement()
    {
        hasBeenAchieved = true;
        OnAchievementAchieved?.Invoke(this, new OnAchievementAchievedEventArgs { achievementSO = achievementSO });
    }
}
