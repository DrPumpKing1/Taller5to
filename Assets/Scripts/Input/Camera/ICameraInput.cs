using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraInput
{
    public bool CanProcessCameraInput();
    public float GetScroll();
}
