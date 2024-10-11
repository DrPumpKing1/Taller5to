using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsDematerializedVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectionResetObject projectionResetObject;

    [Header("Insuficient Projection Gems Settings")]
    [SerializeField] private Transform objectsDematerializedUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private float cooldownTimer;

    private void OnEnable()
    {
        projectionResetObject.OnObjectsDematerialized += ProjectionResetObject_OnObjectsDematerialized;
    }

    private void OnDisable()
    {
        projectionResetObject.OnObjectsDematerialized -= ProjectionResetObject_OnObjectsDematerialized;
    }

    private void Update()
    {
        HandleVisualCooldown();
    }

    private void HandleVisualCooldown()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    private void ProjectionResetObject_OnObjectsDematerialized(object sender, System.EventArgs e)
    {
        if (cooldownTimer > 0) return;

        Transform objectsDematerializedUITransform = Instantiate(objectsDematerializedUIPrefab, transform.position + instantiationPositionOffset, transform.rotation);

        FeedbackUI feedbackUI = objectsDematerializedUITransform.GetComponentInChildren<FeedbackUI>();

        if (!feedbackUI)
        {
            Debug.LogWarning("There's not a FeedbackUI attached to instantiated prefab");
            return;
        }

        cooldownTimer = feedbackUI.TotalLifetime;
    }
}
