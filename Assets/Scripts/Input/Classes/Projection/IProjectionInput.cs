using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectionInput
{
    public bool CanProcessProjectionInput();

    public bool GetNextProjectableObjectDown();
    public bool GetPrevoiusProjectableObjectDown();
}
