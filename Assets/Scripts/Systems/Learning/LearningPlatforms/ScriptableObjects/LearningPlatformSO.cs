using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLearningPlatformSO", menuName = "ScriptableObjects/Learning/LearningPlatform")]
public class LearningPlatformSO : ScriptableObject
{
    public int id;
    public ProjectableObjectSO projectableObjectToLearn;
    public int projectionGemsToAdd;
}
