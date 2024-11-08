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
        bossWeakpoint.OnWeakPointEnable += BossWeakpoint_OnWeakPointEnable;
        bossWeakpoint.OnWeakPointDisable += BossWeakpoint_OnWeakPointDisable;
    }

    private void OnDisable()
    {
        bossWeakpoint.OnWeakPointEnable -= BossWeakpoint_OnWeakPointEnable;
        bossWeakpoint.OnWeakPointDisable -= BossWeakpoint_OnWeakPointDisable;
    }

    private void SetVisual(bool active) => model.SetActive(active);

    private void BossWeakpoint_OnWeakPointEnable(object sender, System.EventArgs e)
    {
        SetVisual(true);
    }

    private void BossWeakpoint_OnWeakPointDisable(object sender, System.EventArgs e)
    {
        SetVisual(false);
    }

}
