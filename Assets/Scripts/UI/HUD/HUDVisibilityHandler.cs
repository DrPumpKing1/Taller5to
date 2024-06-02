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
        FirstObjectLearnedEnd.OnFirstObjectLearnedEnd += FirstObjectLearnedEnd_OnFirstObjectLearnedEnd;
    }
    private void OnDisable()
    {
        FirstObjectLearnedEnd.OnFirstObjectLearnedEnd -= FirstObjectLearnedEnd_OnFirstObjectLearnedEnd;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        CheckIsVisible();
    }

    private void SetIsVisible(bool visible) => isVisible = visible;

    private void ShowHUD()
    {
        ShowHUDInstantly();
    }

    private void HideHUD()
    {
        HideHUDInstantly();
    }

    private void CheckIsVisible()
    {
        if (isVisible)
        {
            ShowHUDInstantly();
        }
        else
        {
            HideHUDInstantly();
        }
    }

    private void ShowHUDInstantly()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
        canvasGroup.blocksRaycasts = true;
    }

    private void HideHUDInstantly()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        canvasGroup.blocksRaycasts = false;
    }


    private void FirstObjectLearnedEnd_OnFirstObjectLearnedEnd(object sender, EventArgs e)
    {
        ShowHUD();
        SetIsVisible(true);
    }
}
