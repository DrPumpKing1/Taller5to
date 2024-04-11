using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProyectableObjectSO", menuName = "ScriptableObjects/Proyection/ProyectableObject")]
public class ProyectableObjectSO : ScriptableObject
{
    [Header("Scriptable Object Settings")]
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
