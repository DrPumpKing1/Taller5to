using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectableObjectUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private int index;

    public int Index => index;

    private const string SELECTION_TRIGGER = "Select";
    private const string DESELECTION_TRIGGER = "Deselect";

    public void SelectUI()
    {
        animator.ResetTrigger(DESELECTION_TRIGGER);
        animator.SetTrigger(SELECTION_TRIGGER);
    }

    public void DeselectUI()
    {
        animator.ResetTrigger(SELECTION_TRIGGER);
        animator.SetTrigger(DESELECTION_TRIGGER);
    }
}
