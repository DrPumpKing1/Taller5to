using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatformLearn : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LearningPlatform learningPlatform;
    [SerializeField] private Transform rotatingObject;

    [Header("Proximity Check")]
    [SerializeField] private float proximityRadius;

    private GameObject player;

    public ProjectableObjectSO ProjectableObjectToLearn => learningPlatform.LearningPlatformSO.projectableObjectToLearn;

    public event EventHandler<OnObjectLearnedEventArgs> OnObjectLearned;

    public static event EventHandler<OnAnyObjectLearnedEventArgs> OnAnyObjectLearned;

    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableOjectSO;
    }

    public class OnAnyObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
        public LearningPlatformSO learningPlatformSO;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckLearn();
    }

    private void InitializeVariables()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void CheckLearn()
    {
        if (learningPlatform.IsLearned) return;
        if (!(Vector3.Distance(transform.position, player.transform.position) <= proximityRadius)) return;

        LearnObject();
        DisableRotatingObject();
    }

    private void DisableRotatingObject() => rotatingObject.gameObject.SetActive(false); 

    private void LearnObject()
    {
        ProjectableObjectsLearningManager.Instance.LearnProjectableObject(ProjectableObjectToLearn);
        ProjectionGemsManager.Instance.IncreaseTotalProjectionGems(learningPlatform.LearningPlatformSO.projectionGemsToAdd);

        learningPlatform.SetIsLearned(true);

        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { projectableOjectSO = ProjectableObjectToLearn });
        OnAnyObjectLearned?.Invoke(this, new OnAnyObjectLearnedEventArgs { projectableObjectSO = ProjectableObjectToLearn, learningPlatformSO = learningPlatform.LearningPlatformSO });
    }
}
