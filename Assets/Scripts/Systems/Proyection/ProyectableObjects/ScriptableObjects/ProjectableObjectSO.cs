using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectableObjectSO", menuName = "ScriptableObjects/Projection/ProjectableObject")]
public class ProjectableObjectSO : ScriptableObject
{
    [Header("Scriptable Object Settings")]
    public Transform prefab;
    public int projectionGemsCost;
    public string objectName;
    public string description;
    public Sprite sprite;
    public Transform visualPrefab;
}
