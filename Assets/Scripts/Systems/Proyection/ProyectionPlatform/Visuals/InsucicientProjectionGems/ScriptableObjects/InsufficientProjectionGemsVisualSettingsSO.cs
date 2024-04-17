using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInsufficientProjectionGemsVisualSettingsSO", menuName = "ScriptableObjects/Projection/ProjectionPlatfofm/InsufficientProyectionGemsVisualSettings")]

public class InsufficientProjectionGemsVisualSettingsSO : ScriptableObject
{
    [Header("Insufficient Projection Gems UI Settings")]
    public Transform insufficientProjectionGemsUIPrefab;
    public Vector3 instantiationPositionOffset;

    [Header("VFX Settings")]
    public Transform insufficientProjectionGemsVFXPrefab;
}
