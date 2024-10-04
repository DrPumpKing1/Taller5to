using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererVisibilityOverrideHandler : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        EnableMeshRenderer();
    }

    private void EnableMeshRenderer() => meshRenderer.enabled = true;
}
