using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionPrefabHolderHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform dragHolder;

    public Transform DragHolder => dragHolder;

    public void ResetDragHolderRotation()
    {
        dragHolder.rotation = Quaternion.identity;
    }
}
