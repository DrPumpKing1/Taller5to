using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningManager : MonoBehaviour
{
    public static LearningManager Instance { get; private set; }

    [SerializeField] private List<ProyectableObjectSO> objectsLearned = new List<ProyectableObjectSO>();

    public EventHandler<OnObjectLearnedEventArgs> OnObjectLearned;

    public class OnObjectLearnedEventArgs
    {
        public ProyectableObjectSO proyectableObjectLearned;
    }

    private void Awake()
    {
        SetSingleton();
    }
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one LearningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void LearnObject(ProyectableObjectSO objectToLearn)
    {
        objectsLearned.Add(objectToLearn);
        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { proyectableObjectLearned = objectToLearn });
    }
}
