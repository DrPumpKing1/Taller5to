using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectableObjectSO", menuName = "ScriptableObjects/ProjectableObject")]
public class ProjectableObjectSO : ScriptableObject
{
    [Header("Scriptable Object Settings")]
    public int id;
    public Transform prefab;
    public int projectionGemsCost;
    public string objectName;
    [TextArea(3, 10)] public string description;
    public Sprite sprite;
    public Transform visualPrefab;
}
