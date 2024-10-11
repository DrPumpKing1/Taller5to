using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JournalInfoPopUpUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator journalInfoPopUpUIAnimator;
    [SerializeField] private Button closeButton;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void Awake()
    {
        AddButtonsListeners();
    }

    private void AddButtonsListeners()
    {
        closeButton.onClick.AddListener(ClosePopUp);
    }

    public void OpenPopUp()
    {
        journalInfoPopUpUIAnimator.ResetTrigger(HIDE_TRIGGER);
        journalInfoPopUpUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    public void ClosePopUp()
    {
        journalInfoPopUpUIAnimator.ResetTrigger(SHOW_TRIGGER);
        journalInfoPopUpUIAnimator.SetTrigger(HIDE_TRIGGER);
    }
}
