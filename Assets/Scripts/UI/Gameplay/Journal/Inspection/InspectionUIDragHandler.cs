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
    private Vector2 defaultAngles;
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
        defaultAngles = Vector2.zero;
        shouldReset = false;
    }

    private void HandleReset()
    {
        if (!shouldReset) return;

        currentAngles.x = Mathf.LerpAngle(currentAngles.x, defaultAngles.x, Time.deltaTime * smoothResetFactor);
        currentAngles.y = Mathf.LerpAngle(currentAngles.y, defaultAngles.y, Time.deltaTime * smoothResetFactor);

        CalculateRotations(currentAngles);
    }

    public void ResetDragHolderRotationInmediately()
    {
        currentAngles = defaultAngles;

        CalculateRotations(currentAngles);
    }

    public void SetDefaultAngles(Vector2 defaultAngles) => this.defaultAngles = defaultAngles;

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

    private void CalculateRotations2(Vector2 newAngles)
    {
        float yawAngle = newAngles.y;
        float pitchAngle = newAngles.x;

        //float pitchAngle = Mathf.Clamp(newAngles.x, -90f, 90f);

        Quaternion yawRotation = Quaternion.AngleAxis(yawAngle, refferenceHolder.up);

        float yawRadians = yawAngle * Mathf.Deg2Rad;  // Convert yaw angle to radians

        // Use sine and cosine to blend between right and forward axes
        Vector3 compensatedPitchAxis = Mathf.Cos(yawRadians) * yawHolder.right + Mathf.Sin(yawRadians) * yawHolder.forward;

        Quaternion pitchRotation = Quaternion.AngleAxis(pitchAngle, compensatedPitchAxis);

        yawHolder.localRotation = yawRotation;
        pitchHolder.localRotation = pitchRotation;

        currentAngles = newAngles;
    }

    private void CalculateRotations(Vector2 angles)
    {
        float yawAngle = angles.y;
        float pitchAngle = angles.x;

        //float pitchAngle = Mathf.Clamp(angles.x, -90f, 90f);

        Quaternion yawRotation = Quaternion.AngleAxis(yawAngle, refferenceHolder.up);
        Quaternion pitchRotation = Quaternion.AngleAxis(pitchAngle, GetPitchCompensatedVector(yawAngle));

        yawHolder.localRotation = yawRotation;
        pitchHolder.localRotation = pitchRotation;

        currentAngles = angles;
    }

    private Vector3 GetPitchCompensatedVector(float yawAngle)
    {
        float yawRadians = yawAngle * Mathf.Deg2Rad;

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
