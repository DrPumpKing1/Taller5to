using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossPlatformDestruction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeToDestroyPlatform;
    [SerializeField] private float graceTimeAfterPlatformDestruction;
    [SerializeField] private List<ProjectionPlatform> projectionPlatforms;
    [SerializeField] private bool randomizeProjectionPlatforms;
    [SerializeField] private bool resetTimerOnPhaseChange;
    [Space]
    [SerializeField] private float timer;

    private bool allPlatformsDestroyed;
    private bool onGraceTime;

    public static event EventHandler OnBossDestroyProjectionPlatform;
    public static event EventHandler OnBossDestroyAllProjectionPlatforms;

    private void OnEnable()
    {
        BossStateHandler.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleProjectionPlatformDestruction();
    }
    private void InitializeVariables()
    {
        ResetTimer();
        allPlatformsDestroyed = false;
        onGraceTime = false;
    }

    private void HandleProjectionPlatformDestruction()
    {
        if (BossStateHandler.Instance.BossState != BossStateHandler.State.OnPhase) return;
        if (allPlatformsDestroyed) return;
        if (onGraceTime) return;

        if (timer > 0) timer -= Time.deltaTime;

        if(timer <= 0)
        {
            DestroyProjectionPlatform();
            ResetTimer();
        }
    }

    private void DestroyProjectionPlatform()
    {
        ProjectionPlatform projectionPlatformToDestroy;

        if (randomizeProjectionPlatforms) projectionPlatformToDestroy = ChooseRandomProjectionPlatform();
        else projectionPlatformToDestroy = ChooseFirstProjectionPlatform();

        projectionPlatforms.Remove(projectionPlatformToDestroy);
        projectionPlatformToDestroy.DestroyProjectionPlatform();

        OnBossDestroyProjectionPlatform?.Invoke(this, EventArgs.Empty);

        ResetTimer();
        StartCoroutine(GraceTimeCoroutine());

        CheckAllPlatformsDestroyed();
    }

    private ProjectionPlatform ChooseRandomProjectionPlatform()
    {
        int randomIndex = UnityEngine.Random.Range(0, projectionPlatforms.Count - 1);
        return projectionPlatforms[randomIndex];
    }

    private ProjectionPlatform ChooseFirstProjectionPlatform() => projectionPlatforms[0];

    private void CheckAllPlatformsDestroyed()
    {
        if (projectionPlatforms.Count != 0) return;

        allPlatformsDestroyed = true;
        OnBossDestroyAllProjectionPlatforms?.Invoke(this, EventArgs.Empty);

        Debug.Log("gg");
    }

    private IEnumerator GraceTimeCoroutine()
    {
        onGraceTime = true;

        yield return new WaitForSeconds(graceTimeAfterPlatformDestruction);

        onGraceTime = false;
    }

    private void ResetTimer() => timer = timeToDestroyPlatform;

    #region BossStateHandler Subscriptions

    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        if (!resetTimerOnPhaseChange) return;
        ResetTimer();
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, EventArgs e)
    {
        if (!resetTimerOnPhaseChange) return;
        ResetTimer();
    }
    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        ResetTimer();
    }
    #endregion
}
