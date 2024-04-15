using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectableObject : MonoBehaviour
{
    [Header("Projectable Object Settings")]
    [SerializeField] private ProjectableObjectSO projectableObjectSO;
    [SerializeField] private ProjectionPlatform projectionPlatform;

    public ProjectableObjectSO ProjectableObjectSO { get { return projectableObjectSO ; } }
    public ProjectionPlatform ProjectionPlatform { get { return projectionPlatform; } }  

    public void SetProjectionPlatform(ProjectionPlatform projectionPlatform) => this.projectionPlatform = projectionPlatform;
}
