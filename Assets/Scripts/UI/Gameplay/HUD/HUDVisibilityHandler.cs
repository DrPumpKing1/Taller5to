using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDVisibilityHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isVisible;

    public bool IsVisible => isVisible;

    private CanvasGroup canvasGroup;

    public event EventHandler OnShowHUD;
    public event EventHandler OnHideHUD;
    public event EventHandler OnShowHUDFirstTime;

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

    public void SetIsVisible(bool visible)
    {
        isVisible = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    private void CheckIsVisible()
    {
        if (isVisible)
        {
            OnShowHUD?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnHideHUD?.Invoke(this, EventArgs.Empty);
        }
    }

    private void FirstObjectLearnedEnd_OnFirstObjectLearnedEnd(object sender, EventArgs e)
    {
        OnShowHUDFirstTime?.Invoke(this, EventArgs.Empty);
        SetIsVisible(true);
    }
}
