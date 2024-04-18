using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
    [SerializeField] private Mode mode;

    private enum Mode { ConstantScale }

    private void LateUpdate()
    {
        ScaleLogic();
    }

    private void ScaleLogic()
    {
        switch (mode)
        {
            case Mode.ConstantScale:
                transform.localScale = CalculateScale();
                break;
        }
    }

    private Vector3 CalculateScale()
    {
        float orthographicSize = Camera.main.orthographicSize;
        float scaleFactor = orthographicSize / CameraScroll.orthoSizeRefference;

        return Vector3.one * scaleFactor;
    }
}
