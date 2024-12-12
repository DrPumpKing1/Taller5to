using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCreditsBackground : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform creditsBGHolderRectTransform;
    [SerializeField] private RectTransform creditsHolderRectTransform;


    [Header("Settings")]
    [SerializeField, Range(0.05f, 0.3f)] private float scrollingFactor;

    private void LateUpdate()
    {
        HandleScrollingCreditsBackground();
    }

    private void HandleScrollingCreditsBackground()
    {
        float targetYPosition = creditsHolderRectTransform.anchoredPosition.y * scrollingFactor;
        creditsBGHolderRectTransform.anchoredPosition = new Vector2(0, targetYPosition);
    }
}
