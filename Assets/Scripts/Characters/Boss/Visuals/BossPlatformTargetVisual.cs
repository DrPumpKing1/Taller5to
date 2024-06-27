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

    private void CreateTargetUI(Vector3 position, float timeTargeting)
    {
        Transform bossPlatformTargetUITransform = Instantiate(bossPlatformTargetUIPrefab, position + instantiationPositionOffset, transform.rotation);
        currentPlatformTargetUI = bossPlatformTargetUITransform;

        BossPlatformTargetUI bossPlatformTargetUI = bossPlatformTargetUITransform.GetComponentInChildren<BossPlatformTargetUI>();

        if (!bossPlatformTargetUI)
        {
            Debug.Log("Instantiated prefab does not have a BossPlatformTargetUI component");
            return;
        }

        bossPlatformTargetUI.SetTimeTargeting(timeTargeting);
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
        CreateTargetUI(e.projectionPlatform.transform.position, e.timeTargeting);
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
