using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionPrefabHolderHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform yawHolder;
    [SerializeField] private Transform pitchHolder;

    public Transform YawHolder => yawHolder;
    public Transform PitchHolder => pitchHolder;
}
