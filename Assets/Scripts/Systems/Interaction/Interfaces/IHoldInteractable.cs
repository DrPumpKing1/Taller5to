using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldInteractable : IInteractable
{
    public float HoldDuration { get; }
}
