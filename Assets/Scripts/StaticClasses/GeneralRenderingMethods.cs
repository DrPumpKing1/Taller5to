using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralRenderingMethods
{
    public static void ChangeMaterialAlpha(Material material, float alpha) => material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

    #region Materials

    public static void SetRendererMaterial(Renderer renderer, Material material) => renderer.material = material;

    public static void SetMaterialEmission(Material material, bool enable)
    {
        if (enable) material.EnableKeyword("_EMISSION");
        else material.DisableKeyword("_EMISSION");
    }
    #endregion

    #region Lights
    public static void SetLightIntensity(Light light, float intensity) => light.intensity = intensity;
    public static void SetLightsIntensity(List<Light> lights, float intensity)
    {
        foreach(Light light in lights)
        {
            SetLightIntensity(light, intensity);
        }
    }

    public static void SetLight(Light light, bool enabled) => light.enabled = enabled;
    public static void SetLightsIntensity(List<Light> lights, bool enabled)
    {
        foreach (Light light in lights)
        {
            SetLight(light, enabled);
        }
    }
    #endregion
}
