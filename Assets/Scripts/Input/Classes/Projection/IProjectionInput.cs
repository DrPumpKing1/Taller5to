using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectionInput
{
    public bool CanProcessProjectionInput();

    public bool GetNextProjectableObjectDown();
    public bool GetPreviousProjectableObjectDown();

    public bool Get1stProjectableObjectDown();
    public bool Get2ndProjectableObjectDown();
    public bool Get3rdProjectableObjectDown();
    public bool Get4thProjectableObjectDown();
    public bool Get5thProjectableObjectDown();

    public bool GetAllProjectableObjectsDematerializationDown();
}
