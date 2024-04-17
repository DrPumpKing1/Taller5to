using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLearningSuccessVisualSettingsSO", menuName = "ScriptableObjects/Learning/LearningPlatform/LearningSucessVisualSettings")]

public class LearningSuccessVisualSettingsSO : ScriptableObject
{
    [Header("Learning Success UI Settings")]
    public Transform learningSuccessUIPrefab;
    public Vector3 instantiationPositionOffset;

    [Header("VFX Settings")]
    public Transform learningSuccessVFXPrefab;
}
