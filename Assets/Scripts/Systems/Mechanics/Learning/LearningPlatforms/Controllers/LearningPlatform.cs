using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatform : MonoBehaviour
{
    [Header("Leraning Platform Settings")]
    [SerializeField] private LearningPlatformSO learningPlatformSO;

    public LearningPlatformSO LearningPlatformSO => learningPlatformSO;

    public bool ObjectHasBeenLearned()
    {
        if (ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned.Contains(learningPlatformSO.projectableObjectToLearn)) return true;
        return false;
    }
}
