using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DataPathsSO dataPathsSO;

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
        if (deleteAchievementsDataOnWin) GeneralDataMethods.DeleteDataInPaths(dataPathsSO.dataPaths);
        else GeneralDataMethods.DeleteDataInPathsExceptAchievements(dataPathsSO.dataPaths);
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToWin) return;
        Win();
    }
}
