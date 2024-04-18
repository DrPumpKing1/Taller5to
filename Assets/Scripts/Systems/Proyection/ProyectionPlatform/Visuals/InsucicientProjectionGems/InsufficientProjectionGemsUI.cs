using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsufficientProjectionGemsUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifeTime;

    public float LifeTime { get { return lifeTime; } }

    private void Awake()
    {
        DestroyAfterLifetime();
    }

    private void DestroyAfterLifetime() => Destroy(transform.parent.gameObject, lifeTime);
}
