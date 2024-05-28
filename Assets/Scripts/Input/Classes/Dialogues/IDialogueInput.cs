using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueInput
{
    public bool CanProcessDialogueInput();
    public bool GetSkipDown();
}

