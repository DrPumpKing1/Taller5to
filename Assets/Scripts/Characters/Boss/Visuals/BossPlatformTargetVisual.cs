using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformTargetVisual : MonoBehaviour
{
    [Header("Platform Target Settings")]
    [SerializeField] private Transform bossPlatformTargetUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private Transform currentPlatformTargetUI;

    private void OnEnable()
    {
        BossObjectDestruction.OnProjectionPlatformTarget += BossObjectDestruction_OnProjectionPlatformTarget;
        BossObjectDestruction.OnProjectionPlatformTargetDestoyed += BossObjectDestruction_OnProjectionPlatformTargetDestoyed;
        BossObjectDestruction.OnProjectionPlatformTargetRemoved += BossObjectDestruction_OnProjectionPlatformTargetRemoved;
        BossObjectDestruction.OnBossAllProjectionGemsLocked += BossObjectDestruction_OnBossAllProjectionGemsLocked;
    }

    private void OnDisable()
    {
        BossObjectDestruction.OnProjectionPlatformTarget -= BossObjectDestruction_OnProjectionPlatformTarget;
        BossObjectDestruction.OnProjectionPlatformTargetDestoyed -= BossObjectDestruction_OnProjectionPlatformTargetDestoyed;
        BossObjectDestruction.OnProjectionPlatformTargetRemoved -= BossObjectDestruction_OnProjectionPlatformTargetRemoved;
        BossObjectDestruction.OnBossAllProjectionGemsLocked -= BossObjectDestruction_OnBossAllProjectionGemsLocked;
    }

    private void CreateTargetUI(Vector3 position)
    {
        Transform bossPlatformTargetUITransform = Instantiate(bossPlatformTargetUIPrefab, position + instantiationPositionOffset, transform.rotation);
        currentPlatformTargetUI = bossPlatformTargetUITransform;
    }

    private void DestroyTargetUI()
    {
        if (!currentPlatformTargetUI) return;

        Destroy(currentPlatformTargetUI.gameObject);
        currentPlatformTargetUI = null;
    }

    private void BossObjectDestruction_OnProjectionPlatformTarget(object sender, BossObjectDestruction.OnProjectionPlatformTargetEventArgs e)
    {
        DestroyTargetUI();
        CreateTargetUI(e.projectionPlatform.transform.position);
    }

    private void BossObjectDestruction_OnProjectionPlatformTargetDestoyed(object sender, BossObjectDestruction.OnProjectionPlatformTargetEventArgs e)
    {
        DestroyTargetUI();
    }

    private void BossObjectDestruction_OnProjectionPlatformTargetRemoved(object sender, BossObjectDestruction.OnProjectionPlatformTargetEventArgs e)
    {
        DestroyTargetUI();
    }

    private void BossObjectDestruction_OnBossAllProjectionGemsLocked(object sender, System.EventArgs e)
    {
        DestroyTargetUI();
    }
}
