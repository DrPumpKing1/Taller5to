using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsufficientProjectionGemsVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectionPlatformProjection projectionPlatformProjection;

    [Header("Insuficient Projection Gems Settings")]
    [SerializeField] private Transform insufficientProjectionGemsUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private float cooldownTimer;

    private void OnEnable()
    {
        projectionPlatformProjection.OnObjectProjectionFailedInsuficientGems += ProjectionPlatformProjection_OnObjectProjectionFailedInsuficientGems;
    }
    private void OnDisable()
    {
        projectionPlatformProjection.OnObjectProjectionFailedInsuficientGems -= ProjectionPlatformProjection_OnObjectProjectionFailedInsuficientGems;
    }

    private void Update()
    {
        HandleVisualCooldown();
    }

    private void HandleVisualCooldown()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    private void ProjectionPlatformProjection_OnObjectProjectionFailedInsuficientGems(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        if (cooldownTimer > 0) return;

        GameObject insufficientProjectionGemsUIGameObject = Instantiate(insufficientProjectionGemsUIPrefab.gameObject, transform.position + instantiationPositionOffset, transform.rotation);

        InsufficientProjectionGemsUI insufficientProjectionGemsUI = insufficientProjectionGemsUIGameObject.GetComponentInChildren<InsufficientProjectionGemsUI>();

        if (!insufficientProjectionGemsUI)
        {
            Debug.LogWarning("There's not a InsufficientProjectionGemsUI attached to instantiated prefab");
            return;
        }

        cooldownTimer = insufficientProjectionGemsUI.LifeTime;
    }

}
