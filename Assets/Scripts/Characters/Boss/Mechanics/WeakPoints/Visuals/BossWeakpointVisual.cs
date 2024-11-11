using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpointVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BossWeakPoint bossWeakpoint;
    [SerializeField] private GameObject model;

    private void OnEnable()
    {
        bossWeakpoint.OnWeakPointEnableA += BossWeakpoint_OnWeakPointEnableA;
        bossWeakpoint.OnWeakPointDisableB += BossWeakpoint_OnWeakPointDisableB;
    }

    private void OnDisable()
    {
        bossWeakpoint.OnWeakPointEnableA -= BossWeakpoint_OnWeakPointEnableA;
        bossWeakpoint.OnWeakPointDisableB -= BossWeakpoint_OnWeakPointDisableB;
    }

    private void SetVisual(bool active) => model.SetActive(active);

    private void BossWeakpoint_OnWeakPointEnableA(object sender, System.EventArgs e)
    {
        SetVisual(true);
    }

    private void BossWeakpoint_OnWeakPointDisableB(object sender, System.EventArgs e)
    {
        SetVisual(false);
    }

}
