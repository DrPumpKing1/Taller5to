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
        showcaseRoomWeakpoint.OnWeakPointEnable += ShowcaseRoomWeakpoint_OnWeakPointEnable;
        showcaseRoomWeakpoint.OnWeakPointDisable += ShowcaseRoomWeakpoint_OnWeakPointDisable;
    }

    private void OnDisable()
    {
        showcaseRoomWeakpoint.OnWeakPointEnable -= ShowcaseRoomWeakpoint_OnWeakPointEnable;
        showcaseRoomWeakpoint.OnWeakPointDisable -= ShowcaseRoomWeakpoint_OnWeakPointDisable;
    }

    private void SetVisual(bool active) => model.SetActive(active);

    private void ShowcaseRoomWeakpoint_OnWeakPointEnable(object sender, System.EventArgs e)
    {
        SetVisual(true);
    }

    private void ShowcaseRoomWeakpoint_OnWeakPointDisable(object sender, System.EventArgs e)
    {
        SetVisual(false);
    }
}
