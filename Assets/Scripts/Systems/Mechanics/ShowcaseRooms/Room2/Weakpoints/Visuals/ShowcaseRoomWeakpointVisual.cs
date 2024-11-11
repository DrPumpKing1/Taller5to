using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomWeakpointVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShowcaseRoomWeakpoint showcaseRoomWeakpoint;
    [SerializeField] private GameObject model;

    private void OnEnable()
    {
        showcaseRoomWeakpoint.OnWeakPointEnableA += ShowcaseRoomWeakpoint_OnWeakPointEnableA;
        showcaseRoomWeakpoint.OnWeakPointDisableB += ShowcaseRoomWeakpoint_OnWeakPointDisableB;
    }

    private void OnDisable()
    {
        showcaseRoomWeakpoint.OnWeakPointEnableA -= ShowcaseRoomWeakpoint_OnWeakPointEnableA;
        showcaseRoomWeakpoint.OnWeakPointDisableB -= ShowcaseRoomWeakpoint_OnWeakPointDisableB;
    }

    private void SetVisual(bool active) => model.SetActive(active);

    private void ShowcaseRoomWeakpoint_OnWeakPointEnableA(object sender, System.EventArgs e)
    {
        SetVisual(true);
    }

    private void ShowcaseRoomWeakpoint_OnWeakPointDisableB(object sender, System.EventArgs e)
    {
        SetVisual(false);
    }
}
