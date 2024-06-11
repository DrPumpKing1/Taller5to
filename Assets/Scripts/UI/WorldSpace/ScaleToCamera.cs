using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Mode mode;

    private enum Mode { ConstantScale}

    private void LateUpdate()
    {
        ScaleLogic();
    }

    private void ScaleLogic()
    {
        switch (mode)
        {
            case Mode.ConstantScale:
                transform.localScale = CameraScrollWorldSpaceUIHandler.Instance.WorldSpaceUIScaleFactor;
                break;
        }
    }
}
