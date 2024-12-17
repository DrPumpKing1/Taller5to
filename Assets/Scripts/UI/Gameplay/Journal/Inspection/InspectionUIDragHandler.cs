using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectionUIDragHandler : MonoBehaviour,IDragHandler
{
    [Header("Components")]
    [SerializeField] private InspectionUIHandler inspectionUIHandler;

    [Header("Settings")]
    [SerializeField,Range(0.1f,3f)] private float dragSensibility;

    public void OnDrag(PointerEventData eventData)
    {
        inspectionUIHandler.InspectionPrefabHolderHandler.DragHolder.localEulerAngles += new Vector3(eventData.delta.y, -eventData.delta.x , 0f) * dragSensibility;
    }
}
