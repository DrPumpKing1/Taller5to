using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectionUIDragHandler : MonoBehaviour,IDragHandler
{
    [Header("Components")]
    [SerializeField] private Transform yawHolder;
    [SerializeField] private Transform pitchHolder;

    [Header("Settings")]
    [SerializeField,Range(0.1f,3f)] private float dragSensibility;

    private Vector2 currentAngles;

    private void Start()
    {
        ResetDragHolderRotation();
    }

    public void ResetDragHolderRotation()
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
        Vector2 newAngles = currentAngles + new Vector2(eventData.delta.y, -eventData.delta.x) * dragSensibility;

        float yawAngle = newAngles.y;
        float pitchAngle = newAngles.x;

        Vector3 yawVector = new Vector3(0f, 1f, 0f);

        Vector3 yawRotation = yawAngle * yawVector;
        Vector3 pitchRotation = pitchAngle * GetPitchCompensatedVector(yawAngle);

        yawHolder.localRotation = Quaternion.Euler(yawRotation);
        pitchHolder.localRotation = Quaternion.Euler(pitchRotation);

        currentAngles = newAngles;
    }

    private Vector3 GetPitchCompensatedVector(float yawAngle)
    {
        float yawRadians = yawAngle * Mathf.PI / 180f;

        Vector3 pitchVector = new Vector3(Mathf.Cos(yawRadians),0f,Mathf.Sin(yawRadians));

        return pitchVector;
    }
}
