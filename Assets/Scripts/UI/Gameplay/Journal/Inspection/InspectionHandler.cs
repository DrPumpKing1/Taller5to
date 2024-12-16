using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform dragHolder;

    public Transform DragHolder => dragHolder;
}
