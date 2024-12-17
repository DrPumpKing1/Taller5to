using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InspectionUIDragHandler : MonoBehaviour,IDragHandler
{
    [Header("Components")]
    [SerializeField] private Button resetButton;
    [Space]
    [SerializeField] private Transform yawHolder;
    [SerializeField] private Transform refferenceHolder;
    [SerializeField] private Transform pitchHolder;

    [Header("Settings")]
    [SerializeField,Range(0.1f,3f)] private float dragSensibility;
    [SerializeField, Range(0.01f, 100f)] private float smoothResetFactor;

    private Vector2 currentAngles;

    private bool shouldReset;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        resetButton.onClick.AddListener(Reset);
    }

    private void Update()
    {
        HandleReset();
    }

    private void Start()
    {
        InitializeVariables();
        ResetDragHolderRotationInmediately();
    }

    private void InitializeVariables()
    {
        shouldReset = false;
    }

    private void HandleReset()
    {
        if (!shouldReset) return;

        currentAngles.x = Mathf.LerpAngle(currentAngles.x, 0f, Time.deltaTime * smoothResetFactor);
        currentAngles.y = Mathf.LerpAngle(currentAngles.y, 0f, Time.deltaTime * smoothResetFactor);

        CalculateRotations(currentAngles);
    }

    public void ResetDragHolderRotationInmediately()
    {
        yawHolder.localEulerAngles = Vector3.zero;
        pitchHolder.localEulerAngles = Vector3.zero;

        currentAngles = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        HandleDrag(eventData);
    }

    private void HandleDrag(PointerEventData eventData)
    {
        shouldReset = false;

        Vector2 newAngles = currentAngles + new Vector2(eventData.delta.y, -eventData.delta.x) * dragSensibility;
        Vector2 normalizedNewAngles = new Vector2(NormalizeAngle(newAngles.x), NormalizeAngle(newAngles.y));

        CalculateRotations(newAngles);
    }

    private void CalculateRotations(Vector2 newAngles)
    {
        float yawAngle = newAngles.y;
        float pitchAngle = newAngles.x;

        Quaternion yawRotation = Quaternion.AngleAxis(yawAngle, refferenceHolder.transform.up);
        Quaternion pitchRotation = Quaternion.AngleAxis(pitchAngle, GetPitchCompensatedVector(yawAngle));

        yawHolder.localRotation = yawRotation;
        pitchHolder.localRotation = pitchRotation;

        currentAngles = newAngles;
    }

    private Vector3 GetPitchCompensatedVector(float yawAngle)
    {
        float yawRadians = yawAngle * Mathf.PI / 180f;

        Vector3 pitchVector = new Vector3(Mathf.Cos(yawRadians),0f,Mathf.Sin(yawRadians));

        return pitchVector;
    }

    private void Reset()
    {
        shouldReset = true;
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;

        if (angle > 360f) angle -= 360f; 
        if (angle < -360f) angle += 360f;  

        return angle;
    }
}
