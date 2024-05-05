using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatform : MonoBehaviour
{
    [Header("Leraning Platform Settings")]
    [SerializeField] private ProjectableObjectSO projectableObjectToLearn;

    [Header("Identifiers")]
    [SerializeField] private int id;
    [SerializeField] private bool isLearned;

    public ProjectableObjectSO ProjectableObjectToLearn => projectableObjectToLearn;
    public int ID => id;
    public bool IsLearned => isLearned;

    public void SetIsLearned() => isLearned = true;
}
