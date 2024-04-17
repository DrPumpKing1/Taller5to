using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRequirementsNotMetVisualSettingsSO", menuName = "ScriptableObjects/Learning/LearningPlatform/RequirementsNotMetVisualSettings")]
public class RequirementsNotMetVisualSettingsSO : ScriptableObject
{
    [Header("Requirements Not Met UI Settings")]
    public Transform requirementsNotMetUIPrefab;
    public Vector3 instantiationPositionOffset;

    [Header("VFX Settings")]
    public Transform requirementsNotMetVFXPrefab;
}
