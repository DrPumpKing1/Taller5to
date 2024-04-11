using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraInput : CameraInput, IActionHandler
{
    [Header("Scroll")]
    private ActionListener zoomAction;

    protected override void Awake()
    {
        base.Awake();
        SetUpActionListener();
    }

    public override bool CanProcessCameraInput() => true;

    public override float GetScroll()
    {
        if (!CanProcessCameraInput()) return 0f;

        float scrollValue = zoomAction.value.f;
        return scrollValue;
    }

    public void SetUpActionListener()
    {
        zoomAction = gameObject.AddComponent<ActionListener>();
        zoomAction.SetActionName("Zoom");
    }
}
