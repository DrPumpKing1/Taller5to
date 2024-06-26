using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IMovementInput 
{
    public bool CanProcessMovementInput();
    public Vector2 GetDirectionVectorNormalized();
    public Vector2 GetIsometricDirectionVectorNormalized();
}
