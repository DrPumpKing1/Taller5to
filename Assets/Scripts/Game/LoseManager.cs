using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoseManager : MonoBehaviour
{
    public static int loseTimes = 0;

    [Header("Settings")]
    [SerializeField,Range(1f,10f)] private float timeToReloadAfterLose;

    public static event EventHandler OnLose;

    private void OnEnable()
    {
        PlayerDefeatedEnd.OnPlayerDefeatedEnd += PlayerDefeatedEnd_OnPlayerDefeatedEnd;
    }

    private void OnDisable()
    {
        PlayerDefeatedEnd.OnPlayerDefeatedEnd -= PlayerDefeatedEnd_OnPlayerDefeatedEnd;
    }

    private void LoseReload()
    {
        StopAllCoroutines();
        StartCoroutine(LoseReloadCoroutine());
    }

    private IEnumerator LoseReloadCoroutine()
    {
        yield return new WaitForSeconds(timeToReloadAfterLose);
        ScenesManager.Instance.FadeReloadCurrentScene();
    }

    private void PlayerDefeatedEnd_OnPlayerDefeatedEnd(object sender, System.EventArgs e)
    {
        LoseReload();

        OnLose?.Invoke(this, EventArgs.Empty);
    }
}
