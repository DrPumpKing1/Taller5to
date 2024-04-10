using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectionPlatformVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProyectionPlatform proyectionPlatform;

    [Header("Visual Settings")]
    [SerializeField] private Renderer selectedVisualRenderer;
    [SerializeField] private Material deselectedMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private float materialFadeTime;

    private State state;
    private enum State { Deselected, Selecting, Deselecting, Selected }

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
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        selectedVisualRenderer.material = deselectedMaterial;

    }


    private void SetVisualMaterial(Material material) => selectedVisualRenderer.material = material;


    #region ProyectionPlatformSubscriptions
    private void ProyectionPlatform_OnObjectSelected(object sender, System.EventArgs e)
    {
        selectedVisualRenderer.material = selectedMaterial;
    }

    private void ProyectionPlatform_OnObjectDeselected(object sender, System.EventArgs e)
    {
        selectedVisualRenderer.material = deselectedMaterial;
    }
    #endregion
}
