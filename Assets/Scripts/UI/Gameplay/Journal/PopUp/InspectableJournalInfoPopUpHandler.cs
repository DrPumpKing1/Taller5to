using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectableJournalInfoPopUpHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button inspectButton;

    [Header("Settings")]
    [SerializeField] private Transform inspectionPrefab;
    [SerializeField] private Color inspectBackgroundColor;

    public static event EventHandler<OnInspectionUIOpenEventArgs> OnInspectionUIOpen;

    public class OnInspectionUIOpenEventArgs : EventArgs
    {
        public Transform inspectionPrefab;
        public Color inspectBackgroundColor;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        inspectButton.onClick.AddListener(OpenInspectUI);
    }

    private void OpenInspectUI()
    {
        OnInspectionUIOpen?.Invoke(this, new OnInspectionUIOpenEventArgs { inspectionPrefab = inspectionPrefab, inspectBackgroundColor = inspectBackgroundColor });
    }
}
