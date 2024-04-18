using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPlatformShowInfo : MonoBehaviour, IInteractableAlternate
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;

    [Header("Interactable Alternate Settings")]
    [SerializeField] private bool canBeSelectedAlternate;
    [SerializeField] private bool isInteractableAlternate;
    [SerializeField] private bool hasAlreadyBeenInteractedAlternate;

    #region IInteractableAlternate Properties
    public bool IsSelectableAlternate => canBeSelectedAlternate;
    public bool IsInteractableAlternate => isInteractableAlternate;
    public bool HasAlreadyBeenInteractedAlternate => hasAlreadyBeenInteractedAlternate;
    public string TooltipMessageAlternate => $"{(!showingInfo ? "Mostrar Información" : "Ocultar Información")}";
    #endregion

    #region IInteractableAlternate Events
    public event EventHandler OnObjectSelectedAlternate;
    public event EventHandler OnObjectDeselectedAlternate;
    public event EventHandler OnObjectInteractedAlternate;
    public event EventHandler OnObjectFailInteractedAlternate;
    public event EventHandler OnObjectHasAlreadyBeenInteractedAlternate;
    #endregion

    public event EventHandler<OnLearningPlatformInfoEventArgs> OnShowInfo;
    public event EventHandler<OnLearningPlatformInfoEventArgs> OnHideInfo;

    public class OnLearningPlatformInfoEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectToLearn;
        public List<DialectKnowledge> dialectKnowledgeRequirements;
    }

    private bool showingInfo;

    private void OnEnable()
    {
        learningPlatform.OnObjectLearned += LearningPlatform_OnObjectLearned;
    }

    private void OnDisable()
    {
        learningPlatform.OnObjectLearned -= LearningPlatform_OnObjectLearned;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        showingInfo = false;
    }

    #region IInteractableAlternateMethods
    public void SelectAlternate()
    {
        OnObjectSelectedAlternate?.Invoke(this, EventArgs.Empty);
        Debug.Log("ProjectableObject Selected Alternate");
    }
    public void DeselectAlternate()
    {
        OnObjectDeselectedAlternate?.Invoke(this, EventArgs.Empty);
        Debug.Log("ProjectableObject Deselected Alternate");

        OnHideInfo?.Invoke(this, new OnLearningPlatformInfoEventArgs { projectableObjectToLearn = learningPlatform.ProjectableObjectToLearn, dialectKnowledgeRequirements = learningPlatform.DialectKnowledgeRequirements });
        showingInfo = false;
    }

    public void TryInteractAlternate()
    {
        if (!isInteractableAlternate)
        {
            FailInteractAlternate();
            return;
        }

        if (hasAlreadyBeenInteractedAlternate)
        {
            AlreadyInteractedAlternate();
            return;
        }

        InteractAlternate();
    }
    public void InteractAlternate()
    {
        ToggleShowInfo();

        Debug.Log("ProjectableObject Interacted Alternate");
        OnObjectInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }

    public void FailInteractAlternate()
    {
        Debug.Log("Cant InteractAlternate with ProjectableObject");
        OnObjectFailInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }

    public void AlreadyInteractedAlternate()
    {
        Debug.Log("ProjectableObject has Already Been Interacted Alternate");
        OnObjectHasAlreadyBeenInteractedAlternate?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetTransform() => transform;

    #endregion

    private void ToggleShowInfo()
    {
        showingInfo = !showingInfo;

        if (showingInfo) OnShowInfo?.Invoke(this, new OnLearningPlatformInfoEventArgs { projectableObjectToLearn = learningPlatform.ProjectableObjectToLearn, dialectKnowledgeRequirements = learningPlatform.DialectKnowledgeRequirements });
        else OnHideInfo?.Invoke(this, new OnLearningPlatformInfoEventArgs { projectableObjectToLearn = learningPlatform.ProjectableObjectToLearn, dialectKnowledgeRequirements = learningPlatform.DialectKnowledgeRequirements });
    }

    #region LearningPlatform Method Subscriptions
    private void LearningPlatform_OnObjectLearned(object sender, LearningPlatform.OnObjectLearnedEventArgs e)
    {
        canBeSelectedAlternate = false;
        isInteractableAlternate = false;
        hasAlreadyBeenInteractedAlternate = true;

        OnHideInfo?.Invoke(this, new OnLearningPlatformInfoEventArgs { projectableObjectToLearn = learningPlatform.ProjectableObjectToLearn, dialectKnowledgeRequirements = learningPlatform.DialectKnowledgeRequirements });
    }
    #endregion
}
