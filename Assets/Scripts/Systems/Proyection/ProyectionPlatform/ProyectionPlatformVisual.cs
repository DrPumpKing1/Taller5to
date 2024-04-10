using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectionPlatformVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProyectionPlatform proyectionPlatform;

    [Header("Visual Settings")]
    [SerializeField] private Renderer visualRenderer;
    [SerializeField] private Material deselectedMaterial;
    [SerializeField] private Material selectedMaterial;

    private void OnEnable()
    {
        proyectionPlatform.OnObjectSelected += ProyectionPlatform_OnObjectSelected;
        proyectionPlatform.OnObjectDeselected += ProyectionPlatform_OnObjectDeselected;
    }
    private void OnDisable()
    {
        proyectionPlatform.OnObjectSelected -= ProyectionPlatform_OnObjectSelected;
        proyectionPlatform.OnObjectDeselected -= ProyectionPlatform_OnObjectDeselected;
    }

    private void Start()
    {
        visualRenderer.material = deselectedMaterial;
    }

    private void ProyectionPlatform_OnObjectSelected(object sender, System.EventArgs e)
    {
        visualRenderer.material = selectedMaterial;
    }

    private void ProyectionPlatform_OnObjectDeselected(object sender, System.EventArgs e)
    {
        visualRenderer.material = deselectedMaterial;
    }

}
