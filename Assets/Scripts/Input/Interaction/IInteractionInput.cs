using System;

public interface IInteractionInput 
{
    public bool CanProcessInteractionInput();
    public bool GetInteractionDown();
    public bool GetInteractionUp();
    public bool GetInteractionHold();
}
