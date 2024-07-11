using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionBarUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private CanvasGroup indicatorCanvasGroup;
    [SerializeField] private Button backgroundButton;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float barValue;

    public Button BackgroundButton => backgroundButton;
    public float BarValue => barValue;

    public void ShowActiveIndicator()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(indicatorCanvasGroup, 1f);
    }
    public void HideActiveIndicator()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(indicatorCanvasGroup, 0f);
    }
}
