using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDoorObjectLearned : ShieldDoor
{
    [Header("Restrictive Object Learned")]
    [SerializeField] private ProjectableObjectSO projectableObjectSO;

    protected override void CheckVirtue()
    {
        isWorthy = ShieldPiecesManager.Instance.HasCompletedShield(dialect) && ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned.Contains(projectableObjectSO);
    }
}
