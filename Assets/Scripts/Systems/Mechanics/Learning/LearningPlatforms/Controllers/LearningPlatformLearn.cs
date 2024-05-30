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

    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableOjectSO;
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
    }
}
