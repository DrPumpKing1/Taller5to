using System;

public interface IInteractionInput 
{
    public bool CanProcessInteractionInput();

    public bool GetInteractionDown();
    public bool GetInteractionUp();
    public bool GetInteractionHold();

    public bool GetInteractionAlternateDown();
    public bool GetInteractionAlternateUp();
    public bool GetInteractionAlternateHold();
}
