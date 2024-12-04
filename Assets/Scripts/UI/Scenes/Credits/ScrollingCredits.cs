using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingCredits : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ScenesInput scenesInput;
    [SerializeField] private RectTransform creditsTransform;

    [Header("Settings")]
    [SerializeField] private float baseScrollSpeed = 20f;
    [SerializeField] private float scrollSpeedMultiplier = 2f;
    [SerializeField] private float anchoredPositionLimit;
    [Space]
    [SerializeField] private string sceneToTransition;

    private bool reachedLimit;

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        reachedLimit = false;
    }
    private void Update()
    {
        ScrollCredits();
        CheckReachedLimit();
    }

    private void ScrollCredits()
    {
        bool skipping = scenesInput.GetSkipHold();

        float speed = skipping? baseScrollSpeed * scrollSpeedMultiplier : baseScrollSpeed;

        creditsTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
    }

    private void CheckReachedLimit()
    {
        if (reachedLimit) return;

        if(creditsTransform.anchoredPosition.y >= anchoredPositionLimit)
        {
            ScenesManager.Instance.FadeLoadTargetScene(sceneToTransition);
            reachedLimit = true;
        }
    }
}
