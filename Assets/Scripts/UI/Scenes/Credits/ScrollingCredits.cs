using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingCredits : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform creditsTransform;

    [Header("Settings")]
    [SerializeField] private float scrollSpeed = 20f;
    [SerializeField] private float anchoredPositionLimit;
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
        creditsTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
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
