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
    [Space]
    [SerializeField] private Color lightColor;
    [SerializeField,Range(5f,40f)] private float lightIntensity;
    [SerializeField] private Vector2 inspectionDefaultAngles;
    [SerializeField] private Sprite inspectionBGSprite;

    public static event EventHandler<OnInspectionUIOpenEventArgs> OnInspectionUIOpen;

    public class OnInspectionUIOpenEventArgs : EventArgs
    {
        public Transform inspectionPrefab;
        public Color inspectBackgroundColor;
        public Color lightColor;
        public float lightIntensity;
        public Vector2 inspectionDefaultAngles;
        public Sprite inspectionBGSprite;
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
        OnInspectionUIOpen?.Invoke(this, new OnInspectionUIOpenEventArgs { inspectionPrefab = inspectionPrefab, inspectBackgroundColor = inspectBackgroundColor, lightColor = lightColor, lightIntensity= lightIntensity, inspectionDefaultAngles = inspectionDefaultAngles, inspectionBGSprite = inspectionBGSprite});
    }
}
