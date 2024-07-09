using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoseManager : MonoBehaviour
{
    public static int loseTimes = 0;

    [Header("Settings")]
    [SerializeField] private string logToLose;
    [SerializeField,Range(1f,10f)] private float timeToReloadAfterLose;

    public static event EventHandler OnLose;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Lose()
    {
        StopAllCoroutines();
        StartCoroutine(LoseCoroutine());
    }

    private IEnumerator LoseCoroutine()
    {
        yield return new WaitForSeconds(timeToReloadAfterLose);
        ScenesManager.Instance.FadeReloadCurrentScene();
    }


    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToLose) return;
        Lose();
        OnLose?.Invoke(this, EventArgs.Empty);
    }

}
