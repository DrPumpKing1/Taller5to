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
        bossWeakpoint.OnWeakPointEnableB += BossWeakpoint_OnWeakPointEnableB;
        bossWeakpoint.OnWeakPointDisableA += BossWeakpoint_OnWeakPointDisableA;
    }

    private void OnDisable()
    {
        bossWeakpoint.OnWeakPointEnableB -= BossWeakpoint_OnWeakPointEnableB;
        bossWeakpoint.OnWeakPointDisableA -= BossWeakpoint_OnWeakPointDisableA;
    }

    private void SetVisual(bool active) => model.SetActive(active);

    private void BossWeakpoint_OnWeakPointEnableB(object sender, System.EventArgs e)
    {
        SetVisual(true);
    }

    private void BossWeakpoint_OnWeakPointDisableA(object sender, System.EventArgs e)
    {
        SetVisual(false);
    }

}
