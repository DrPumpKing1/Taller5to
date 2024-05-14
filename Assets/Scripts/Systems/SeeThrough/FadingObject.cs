using System;
using System.Collections.Generic;
using UnityEngine;

public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
{
    [Header("Settings")]
    [SerializeField,Range(0f,1f)] private float fadeAlpha;
    [SerializeField,Range(0.1f,3f)] private float fadeTime;

    [HideInInspector] public List<Material> materials = new List<Material>();

    private List<Renderer> renderers = new List<Renderer>();
    private Vector3 position;

    public float InitialAlpha { get; private set; }
    public float FadeAlpha => fadeAlpha;
    public float FadeTime => fadeTime;

    private void Awake()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        position = transform.position;

        if (renderers.Count == 0)
        {
            renderers.AddRange(GetComponentsInChildren<Renderer>());
        }
        foreach(Renderer renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }

        InitialAlpha = materials[0].color.a;
    }

    public bool Equals(FadingObject other)
    {
        return position.Equals(other.position);
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}
