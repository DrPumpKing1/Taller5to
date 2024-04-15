using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InputCapturer : MonoBehaviour
{
    [SerializeField] private string codeName;

    public string CodeName { get { return codeName; } }

    protected virtual void Update()
    {
        GetInput();
    }

    protected abstract void GetInput();

    public abstract InputValue SendInput();
}
