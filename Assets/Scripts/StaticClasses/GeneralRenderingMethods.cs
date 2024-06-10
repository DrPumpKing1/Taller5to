using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralRenderingMethods
{
    public static void ChangeMaterialAlpha(Material material, float alpha) => material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

    public static void SetMaterialEmission(Material material, bool enable)
    {
        if (enable) material.EnableKeyword("_EMISSION");
        else material.DisableKeyword("_EMISSION");
    }
}
