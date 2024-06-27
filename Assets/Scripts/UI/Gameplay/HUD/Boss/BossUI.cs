using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private Image bossBar;

    [Header("Settings")]
    [SerializeField] private bool fillUntilEnd;
    [Space]
    [SerializeField, Range(1f,100f)] private float regularSmoothFillFactor;
    [SerializeField, Range(1f, 100f)] private float phaseChangeSmoothFillFactor;
    [Space]
    [SerializeField] private Color phaseColor1;
    [SerializeField] private Color phaseColor2;
    [SerializeField] private Color phaseColor3;

    [Header("Debug")]
    [SerializeField] private float targetFillAmount;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private float smoothFillFactor;
    private float currentFillAmount;
    
    private void OnEnable()
    {
        BossStateHandler.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossKaerumOvercharge.OnCurrentHitsInPhaseChanged += BossKaerumOvercharge_OnCurrentHitsInPhaseChanged;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart; ;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;

        BossKaerumOvercharge.OnCurrentHitsInPhaseChanged -= BossKaerumOvercharge_OnCurrentHitsInPhaseChanged;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleFillAmount();
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetImageFillRatio(bossBar,0f);
        SetSmoothFillFactor(regularSmoothFillFactor);
        SetTargetFillAmount(0f);
        ChangeBarColor(1);

        currentFillAmount = targetFillAmount;
    }

    private void HandleFillAmount()
    {
        currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, smoothFillFactor * Time.deltaTime);
        GeneralUIMethods.SetImageFillRatio(bossBar, currentFillAmount);
    }

    private void ChangeBarColor(int phaseNumber)
    {
        Color colorToChange;

        switch (phaseNumber)
        {
            case 1:
                colorToChange = phaseColor1;
                break;
            case 2:
                colorToChange = phaseColor2;
                break;
            case 3:
                colorToChange = phaseColor3;
                break;
            default:
                colorToChange = phaseColor1;
                break;
        }

        GeneralUIMethods.SetImageColor(bossBar, colorToChange);
    }

    private void ShowBossBar()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideBossBar()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void ChangeTargetFillAmount(int numerator, int denominator)
    {
        if (fillUntilEnd && denominator > 1) denominator--;

        targetFillAmount = (float)numerator / denominator;
    }

    private void SetTargetFillAmount(float fillAmount) => targetFillAmount = fillAmount;
    private void SetSmoothFillFactor(float smoothFillFactor) => this.smoothFillFactor = smoothFillFactor;


    private void BossKaerumOvercharge_OnCurrentHitsInPhaseChanged(object sender, BossKaerumOvercharge.OnCurrentHitsInPhaseChangedEventArgs e)
    {
        ChangeTargetFillAmount(e.currentHitsInPhase, e.hitsPerPhase);
    }


    private void BossStateHandler_OnBossActiveStart(object sender, System.EventArgs e)
    {
        ShowBossBar();
    }

    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e)
    {
        HideBossBar();
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        SetTargetFillAmount(0f);
        SetSmoothFillFactor(phaseChangeSmoothFillFactor);
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        ChangeBarColor(e.phaseNumber);
        SetSmoothFillFactor(regularSmoothFillFactor);
    }

}
