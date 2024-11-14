using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string logToWin;
    [SerializeField] private string transitionScene;
    [SerializeField, Range(1f, 10f)] private float timeToTransitionAfterWin;
    [Space]
    [SerializeField] private bool deleteAchievementsDataOnWin;

    public static event EventHandler OnWin;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Win()
    {
        StopAllCoroutines();
        StartCoroutine(WinCoroutine());
    }
    private IEnumerator WinCoroutine()
    {
        OnWin?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(timeToTransitionAfterWin);
        ScenesManager.Instance.FadeLoadTargetScene(transitionScene);

        DeleteAllData();
    }

    private void DeleteAllData()
    {
        PlayerDataPersistenceManager.Instance.DeleteGameData();
        PetDataPersistenceManager.Instance.DeleteGameData();
        ObjectsDataPersistenceManager.Instance.DeleteGameData();
        UIDataPersistenceManager.Instance.DeleteGameData();
        LogDataPersistenceManager.Instance.DeleteGameData();
        JournalDataPersistenceManager.Instance.DeleteGameData();

        if(deleteAchievementsDataOnWin) AchievementsDataPersistenceManager.Instance.DeleteGameData();
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToWin) return;
        Win();
    }
}
