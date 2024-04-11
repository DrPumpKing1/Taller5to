using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlatformVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectionPlatform projectionPlatform;

    [Header("Visual Settings")]
    [SerializeField] private Renderer selectedVisualRenderer;
    [Space]
    [SerializeField] private Material deselectedVisualMaterial;
    [SerializeField] private Material selectedVisualMaterial;
    [Space]
    [SerializeField, Range(0f,0.5f)] private float materialFadeTime;
    [SerializeField, Range(0f,1f)] private float minAlpha = 0;
    [SerializeField, Range(0f,1f)] private float maxAlpha = 1;

    private void OnEnable()
    {
        projectionPlatform.OnObjectSelected += ProyectionPlatform_OnObjectSelected;
        projectionPlatform.OnObjectDeselected += ProyectionPlatform_OnObjectDeselected;
    }
    private void OnDisable()
    {
        projectionPlatform.OnObjectSelected -= ProyectionPlatform_OnObjectSelected;
        projectionPlatform.OnObjectDeselected -= ProyectionPlatform_OnObjectDeselected;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        selectedVisualRenderer.material.SetOverrideTag("RenderType", "Transparent");
        SetVisualMaterial(deselectedVisualMaterial);
        SetVisualMaterialAlpha(minAlpha);
    }

    private IEnumerator SelectProyectionPlatform()
    {
        SetVisualMaterial(selectedVisualMaterial);

        float time = 0;
        float startingAlpha = selectedVisualRenderer.material.color.a;

        while (selectedVisualRenderer.material.color.a < maxAlpha)
        {
            selectedVisualRenderer.material.color = new Color(selectedVisualRenderer.material.color.r, selectedVisualRenderer.material.color.g, selectedVisualRenderer.material.color.b, Mathf.Lerp(startingAlpha, maxAlpha, time * 1 / materialFadeTime));
            time += Time.deltaTime;
            yield return null;
        }

        SetVisualMaterialAlpha(maxAlpha);
    }

    private IEnumerator DeselelectProyectionPlatform()
    {
        float time = 0;
        float startingAlpha = selectedVisualRenderer.material.color.a;

        while (selectedVisualRenderer.material.color.a > minAlpha)
        {
            selectedVisualRenderer.material.color = new Color(selectedVisualRenderer.material.color.r, selectedVisualRenderer.material.color.g, selectedVisualRenderer.material.color.b, Mathf.Lerp(startingAlpha, minAlpha, time * 1 / materialFadeTime));
            time += Time.deltaTime;
            yield return null;
        }

        SetVisualMaterial(deselectedVisualMaterial);
        SetVisualMaterialAlpha(minAlpha);
    }

    private void SetVisualMaterial(Material material) => selectedVisualRenderer.material = material;
    private void SetVisualMaterialAlpha(float alpha) => selectedVisualRenderer.material.color = new Color(selectedVisualRenderer.material.color.r, selectedVisualRenderer.material.color.g, selectedVisualRenderer.material.color.b, alpha);

    #region ProyectionPlatformSubscriptions
    private void ProyectionPlatform_OnObjectSelected(object sender, System.EventArgs e)
    {
        //selectedVisualRenderer.material = selectedVisualMaterial;
        StopAllCoroutines();
        StartCoroutine(SelectProyectionPlatform());
    }

    private void ProyectionPlatform_OnObjectDeselected(object sender, System.EventArgs e)
    {
        //DelectedVisualRenderer.material = deselectedVisualMaterial;
        StopAllCoroutines();
        StartCoroutine(DeselelectProyectionPlatform());
    }
    #endregion
}
