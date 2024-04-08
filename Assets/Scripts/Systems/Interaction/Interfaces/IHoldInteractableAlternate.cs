using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldInteractableAlternate : IInteractableAlternate
{
    public float HoldDurationAlternate { get; }

    public bool CheckSuccessAlternate();
}
