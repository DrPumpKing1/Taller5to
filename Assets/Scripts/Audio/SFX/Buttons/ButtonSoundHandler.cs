using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundHandler : MonoBehaviour
{
    #region Relugar Buttons
    public void PlaySFXButtonClick1()
    {
        if (!UnPausableSFXManager.Instance) return;
        UnPausableSFXManager.Instance.PlaySFXButtonClick1();
    }

    public void PlaySFXButtonClick2()
    {
        if (!UnPausableSFXManager.Instance) return;
        UnPausableSFXManager.Instance.PlaySFXButtonClick2();
    }

    public void PlaySFXButtonClick3()
    {
        if (!UnPausableSFXManager.Instance) return;
        UnPausableSFXManager.Instance.PlaySFXButtonClick3();
    }

    #endregion

    #region Journal Buttons
    public void PlaySFXJournalButtonClick1()
    {
        if (!UnPausableSFXManager.Instance) return;
        UnPausableSFXManager.Instance.PlaySFXButtonClick3();
    }

    public void PlaySFXJournalButtonClick2()
    {
        if (!UnPausableSFXManager.Instance) return;
        UnPausableSFXManager.Instance.PlaySFXButtonClick3();
    }

    public void PlaySFXJournalButtonClick3()
    {
        if (!UnPausableSFXManager.Instance) return;
        UnPausableSFXManager.Instance.PlaySFXButtonClick3();
    }
    #endregion
}
