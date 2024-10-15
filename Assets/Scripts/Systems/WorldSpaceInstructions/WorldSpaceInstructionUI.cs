using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceInstructionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator worldSpaceUIAnimator;
    [SerializeField] private GameObject parent;

    private const float TIME_TO_DESTROY = 1f;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";


    private void Start()
    {
        ShowInstructionUI();
    }

    public void AcomplishInstruction()
    {
        HideInstructionUI();
        Destroy(parent, TIME_TO_DESTROY);
    }

    private void ShowInstructionUI()
    {
        worldSpaceUIAnimator.ResetTrigger(HIDE_TRIGGER);
        worldSpaceUIAnimator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideInstructionUI()
    {
        worldSpaceUIAnimator.ResetTrigger(SHOW_TRIGGER);
        worldSpaceUIAnimator.SetTrigger(HIDE_TRIGGER);
    }
}
