using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSourceStunVisual : MonoBehaviour
{
    [Header("Platform Target Settings")]
    [SerializeField] private Transform bossSourceStunUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private Transform currentSourceStunUI;

    private void OnEnable()
    {
        BossSourceStun.OnSourceStunned += BossSourceStun_OnSourceStunned;
        BossSourceStun.OnSourceStunnedEnd += BossSourceStun_OnSourceStunnedEnd;
        BossObjectDestruction.OnBossAllProjectionGemsLocked += BossObjectDestruction_OnBossAllProjectionGemsLocked;
    }

    private void OnDisable()
    {
        BossSourceStun.OnSourceStunned -= BossSourceStun_OnSourceStunned;
        BossSourceStun.OnSourceStunnedEnd -= BossSourceStun_OnSourceStunnedEnd; ;
        BossObjectDestruction.OnBossAllProjectionGemsLocked -= BossObjectDestruction_OnBossAllProjectionGemsLocked;
    }

    private void CreateStunUI(Vector3 position, float timeTargeting)
    {
        Transform bossSourceStunUITransform = Instantiate(bossSourceStunUIPrefab, position + instantiationPositionOffset, transform.rotation);
        currentSourceStunUI = bossSourceStunUITransform;

        BossSourceStunUI bossSourceStuUI = bossSourceStunUITransform.GetComponentInChildren<BossSourceStunUI>();

        if (!bossSourceStuUI)
        {
            Debug.Log("Instantiated prefab does not have a BossSourceStunUI component");
            return;
        }

        bossSourceStuUI.SetTimeStunning(timeTargeting);
    }

    private void DestroyStunUI()
    {
        if (!currentSourceStunUI) return;

        Destroy(currentSourceStunUI.gameObject);
        currentSourceStunUI = null;
    }

    private void BossSourceStun_OnSourceStunned(object sender, BossSourceStun.OnSourceStunnedEventArgs e)
    {
        DestroyStunUI();
        CreateStunUI(e.stuneableSource.transform.position, e.timeStunning);
    }

    private void BossSourceStun_OnSourceStunnedEnd(object sender, BossSourceStun.OnSourceStunnedEventArgs e)
    {
        DestroyStunUI();
    }

    private void BossObjectDestruction_OnBossAllProjectionGemsLocked(object sender, System.EventArgs e)
    {
        DestroyStunUI();
    }
}
