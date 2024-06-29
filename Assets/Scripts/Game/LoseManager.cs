using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseManager : MonoBehaviour
{
    public static int loseTimes = 0;

    [Header("Settings")]
    [SerializeField,Range(1f,10f)] private float timeToReloadAfterLose;

    private void OnEnable()
    {
        BossStateHandler.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;
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

    private void BossStateHandler_OnPlayerDefeated(object sender, System.EventArgs e)
    {
        LoseReload();
    }
}
