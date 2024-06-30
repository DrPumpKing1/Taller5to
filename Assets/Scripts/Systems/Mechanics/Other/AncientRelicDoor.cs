using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AncientRelicDoor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform transformToDisable;

    [Header("Settings")]
    [SerializeField] private List<Electrode> controllingElectrodes;
}
