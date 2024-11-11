using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossFightDetection : MonoBehaviour
{
    [Header("Logs")]
    [SerializeField] private string logToAwakeBoss;
    [SerializeField] private string logToDefeatBoss;

    [Header("OtherLogs")]
    [SerializeField] private List<string> logsToQuitMidFight;
    [SerializeField] private List<string> logsToReturnMidFight;

    private bool bossAwaken;
    private bool bossDefeated;
    private bool onFight;

    public static event EventHandler OnBossAwaken;
    public static event EventHandler OnBossStartFight;
    public static event EventHandler OnBossEndFight;
    public static event EventHandler OnBossDefeated;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }
    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        bossAwaken = false;
        bossDefeated = false;
        onFight = false;
    }

    private void CheckBossAwaken(string log)
    {
        if(bossAwaken) return;
        if (log != logToAwakeBoss) return;

        bossAwaken = true;
        onFight=true;
        OnBossAwaken?.Invoke(this, EventArgs.Empty);
        OnBossStartFight?.Invoke(this, EventArgs.Empty);
    }

    private void CheckBossDefeated(string log)
    {
        if (bossDefeated) return;
        if (log != logToDefeatBoss) return;

        bossDefeated = true;
        onFight=false;
        OnBossDefeated?.Invoke(this, EventArgs.Empty);
        OnBossEndFight?.Invoke(this, EventArgs.Empty);
    }
    private void CheckBossQuitMidFight(string log)
    {
        if (!bossAwaken) return;
        if (!onFight) return;

        foreach(string logToQuitMidFight in logsToQuitMidFight)
        {
            if (logToQuitMidFight != log) continue;

            onFight = false;
            OnBossEndFight?.Invoke(this, EventArgs.Empty);
            return;
        }
    }

    private void CheckBossReturnMidFight(string log)
    {
        if (!bossAwaken) return;
        if (onFight) return;

        foreach (string logToReturnMidFight in logsToReturnMidFight)
        {
            if (logToReturnMidFight != log) continue;

            onFight = true;
            OnBossStartFight?.Invoke(this, EventArgs.Empty);
            return;
        }
    }

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckBossAwaken(e.gameplayAction.log);
        CheckBossDefeated(e.gameplayAction.log);

        CheckBossQuitMidFight(e.gameplayAction.log);
        CheckBossReturnMidFight(e.gameplayAction.log);
    }
    #endregion
}
