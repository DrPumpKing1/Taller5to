using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatform : MonoBehaviour
{
    [Header("Leraning Platform Settings")]
    [SerializeField] private LearningPlatformSO learningPlatformSO;

    [Header("Identifiers")]
    [SerializeField] private bool isLearned;

    public LearningPlatformSO LearningPlatformSO => learningPlatformSO;
    public bool IsLearned => isLearned;

    public void SetIsLearned() => isLearned = true;
}
