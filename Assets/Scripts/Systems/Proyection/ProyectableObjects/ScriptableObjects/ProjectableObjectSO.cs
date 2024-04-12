using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProyectableObjectSO", menuName = "ScriptableObjects/Proyection/ProyectableObject")]
public class ProjectableObjectSO : ScriptableObject
{
    [Header("Scriptable Object Settings")]
    public Transform prefab;
    public int projectionGemsCost;
    public Sprite sprite;
    public string objectName;
}
