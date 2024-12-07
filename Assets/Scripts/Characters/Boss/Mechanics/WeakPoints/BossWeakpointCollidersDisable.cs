using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpointCollidersDisable : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<BoxCollider> collidersToDisableAlmostDefeated;
    [SerializeField] private List<BoxCollider> collidersToDisableDefeated;

    private void OnEnable()
    {
        BossStateHandler.OnBossAlmostDefeated += BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossAlmostDefeated -= BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
    }

    private void DisableColliders(List<BoxCollider> colliders)
    {
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void BossStateHandler_OnBossAlmostDefeated(object sender, System.EventArgs e)
    {
        DisableColliders(collidersToDisableAlmostDefeated);
    }

    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e)
    {
        DisableColliders(collidersToDisableDefeated);
    }
}
