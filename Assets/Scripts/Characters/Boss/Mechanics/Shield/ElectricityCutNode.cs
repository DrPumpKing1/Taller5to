using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityCutNode : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform nodeTransform;
    [SerializeField] private Transform powerPosition;
    [SerializeField] private Transform cutPosition;

    [Header("Settings")]
    [SerializeField] private bool startCut;

    private void Start()
    {
        InitializeNode();
    }

    private void InitializeNode()
    {
        if (startCut) CutElectricity();
        else LetElectricity();
    }

    public void CutElectricity() => MoveNode(cutPosition);
    public void LetElectricity() => MoveNode(powerPosition);

    private void MoveNode(Transform transform) => nodeTransform.transform.position = transform.position;

}
