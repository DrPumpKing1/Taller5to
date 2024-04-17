using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHoldInteractable : IInteractable
{
    public float HoldDuration { get; }

    public event EventHandler OnHoldInteractionStart;
    public event EventHandler OnHoldInteractionEnd;
    public event EventHandler<OnHoldInteractionEventArgs> OnContinousHoldInteraction;

    public class OnHoldInteractionEventArgs : EventArgs
    {
        public float holdTimer;
        public float holdDuration;
    }

    public bool CheckSuccess();
    public void HoldInteractionStart();
    public void ContinousHoldInteraction(float holdTimer);
    public void HoldInteractionEnd();
}
