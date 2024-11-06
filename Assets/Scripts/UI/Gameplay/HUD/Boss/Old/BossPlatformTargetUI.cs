using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BossPlatformTargetUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI timeRemainingText;

    private float timeTargeting;
    private float timeRemainingRaw;

    private int timeRemaining;
    private int previousTimeRemaining;


    public void SetTimeTargeting(float timeTargeting) 
    {
        this.timeTargeting = timeTargeting;
        timeRemainingRaw = timeTargeting;

        timeRemaining = Mathf.CeilToInt(timeRemainingRaw);
        previousTimeRemaining = timeRemaining;

        UpdateTimeRemainingText();
    }

    private void Update()
    {
        HandleTimeRemaining();
    }

    private void HandleTimeRemaining()
    {
        if (timeRemainingRaw <= 0f) return;

        timeRemainingRaw -= Time.deltaTime;
        timeRemaining = Mathf.CeilToInt(timeRemainingRaw);

        if(timeRemaining < previousTimeRemaining)
        {
            UpdateTimeRemainingText();
            previousTimeRemaining = timeRemaining;
        }
    }

    private void UpdateTimeRemainingText()
    {
        if (timeRemaining <= 0) return;
        timeRemainingText.text = timeRemaining.ToString();
    }
}
