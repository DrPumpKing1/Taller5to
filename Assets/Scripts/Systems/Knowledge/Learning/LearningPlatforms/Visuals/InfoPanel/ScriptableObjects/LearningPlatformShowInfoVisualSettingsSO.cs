using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLearningPlatformShowInfoVisualSettingsSO", menuName = "ScriptableObjects/Learning/LearningPlatform/LearningPlatformShowInfoVisualSettings")]

public class LearningPlatformShowInfoVisualSettingsSO : ScriptableObject
{
    [Header("Show Info UI Settings")]
    public Transform infoPanelUIPrefab;
    public Vector3 instantiationPositionOffset;

    [Header("VFX Settings")]
    public Transform showInfoVFXPrefab;
}
