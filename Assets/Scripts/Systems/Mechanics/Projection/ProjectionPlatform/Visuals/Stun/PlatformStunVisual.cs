using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStunVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StunableProjectionPlatformProjection stunableProjectionPlatformProjection;

    [Header("Insuficient Projection Gems Settings")]
    [SerializeField] private Transform platformStunUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private float cooldownTimer;

    private void OnEnable()
    {
        stunableProjectionPlatformProjection.OnProjectionPlatformFailInteractStun += StunableProjectionPlatformProjection_OnProjectionPlatformFailInteractStun; ;
    }

    private void OnDisable()
    {
        stunableProjectionPlatformProjection.OnProjectionPlatformFailInteractStun -= StunableProjectionPlatformProjection_OnProjectionPlatformFailInteractStun;
    }

    private void Update()
    {
        HandleVisualCooldown();
    }

    private void HandleVisualCooldown()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    private void StunableProjectionPlatformProjection_OnProjectionPlatformFailInteractStun(object sender, System.EventArgs e)
    {
        if (cooldownTimer > 0) return;

        Transform insufficientProjectionGemsUITransform = Instantiate(platformStunUIPrefab, transform.position + instantiationPositionOffset, transform.rotation);

        FeedbackUI feedbackUI = insufficientProjectionGemsUITransform.GetComponentInChildren<FeedbackUI>();

        if (!feedbackUI)
        {
            Debug.LogWarning("There's not a FeedbackUI attached to instantiated prefab");
            return;
        }

        cooldownTimer = feedbackUI.TotalLifetime;
    }
}
