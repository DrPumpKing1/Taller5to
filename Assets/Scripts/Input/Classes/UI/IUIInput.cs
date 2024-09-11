using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIInput
{
    public bool CanProcessUIInput();

    public bool GetPauseDown();
    public bool GetInventoryDown();
    public bool GetJournalDown();
}
