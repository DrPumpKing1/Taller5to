using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHoldInteractableAlternate : IInteractableAlternate
{
    public float HoldDurationAlternate { get; }

    public event EventHandler OnHoldInteractionAlternateStart;
    public event EventHandler OnHoldInteractionAlternateEnd;
    public event EventHandler<OnHoldInteractionAlternateEventArgs> OnContinousHoldInteractionAlternate;

    public class OnHoldInteractionAlternateEventArgs : EventArgs
    {
        public float holdTimer;
    }
    public bool CheckSuccessAlternate();

    public void HoldInteractionAlternateStart();
    public void ContinousHoldInteractionAlternate(float holdTimer);
    public void HoldInteractionAlternateEnd();
}
