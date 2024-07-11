using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalProjectileVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SignalProjectile signalProjectile;

    [Header("VisualEffects")]
    [SerializeField] private Transform trailTransform;
    [SerializeField] private Transform impactTransform;

    [Header("Settings")]
    [SerializeField,Range(0f,1f)] private float trailDestroyTime;

    private void OnEnable()
    {
        signalProjectile.OnProjectileImpact += SignalProjectile_OnProjectileImpact;
        signalProjectile.OnProjectileLifespanEnd += SignalProjectile_OnProjectileLifespanEnd;
    }

    private void OnDisable()
    {
        signalProjectile.OnProjectileImpact -= SignalProjectile_OnProjectileImpact;
        signalProjectile.OnProjectileLifespanEnd -= SignalProjectile_OnProjectileLifespanEnd;
    }

    private void UnparentTrail() => trailTransform.SetParent(null);
    private void DestroyTrailAfterTime() => Destroy(trailTransform.gameObject, trailDestroyTime);

    private void SignalProjectile_OnProjectileImpact(object sender, System.EventArgs e)
    {
        UnparentTrail();
        DestroyTrailAfterTime();
    }

    private void SignalProjectile_OnProjectileLifespanEnd(object sender, System.EventArgs e)
    {
        UnparentTrail();
        DestroyTrailAfterTime();
    }
}
