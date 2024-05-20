using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoObjectsToDematerializeVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectionResetObject projectionResetObject;

    [Header("Insuficient Projection Gems Settings")]
    [SerializeField] private Transform noObjectsToDematerializeUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private float cooldownTimer;

    private void OnEnable()
    {
        projectionResetObject.OnNoObjectsToDematerialize += ProjectionResetObject_OnNoObjectsToDematerialize;
    }

    private void OnDisable()
    {
        projectionResetObject.OnNoObjectsToDematerialize -= ProjectionResetObject_OnNoObjectsToDematerialize;
    }

    private void Update()
    {
        HandleVisualCooldown();
    }

    private void HandleVisualCooldown()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    private void ProjectionResetObject_OnNoObjectsToDematerialize(object sender, System.EventArgs e)
    {
        if (cooldownTimer > 0) return;

        Transform noObjectsToDematerializeUITransform = Instantiate(noObjectsToDematerializeUIPrefab, transform.position + instantiationPositionOffset, transform.rotation);

        NoObjectsToDematerializeUI noObjectsToDematerializeUI = noObjectsToDematerializeUITransform.GetComponentInChildren<NoObjectsToDematerializeUI>();

        if (!noObjectsToDematerializeUI)
        {
            Debug.LogWarning("There's not a InsufficientProjectionGemsUI attached to instantiated prefab");
            return;
        }

        cooldownTimer = noObjectsToDematerializeUI.LifeTime;
    }
}
