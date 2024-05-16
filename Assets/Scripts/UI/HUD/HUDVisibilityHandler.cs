using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDVisibilityHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isVisible;

    private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        //Subscribe to event to showHud;
    }

    private void OnDisable()
    {
        //Unsubscribe to event to showHud;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        CheckIsVisible();
    }

    private void SetIsVisible() => isVisible = true;

    private void CheckIsVisible()
    {
        if (isVisible)
        {
            ShowInstantly();
        }
        else
        {
            HideInstantly();
        }
    }

    private void ShowInstantly()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.blocksRaycasts = true;
    }

    private void HideInstantly()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.blocksRaycasts = false;
    }
}
