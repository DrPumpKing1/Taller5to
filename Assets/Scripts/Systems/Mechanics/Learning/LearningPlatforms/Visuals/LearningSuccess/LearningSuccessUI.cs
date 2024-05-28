using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LearningSuccessUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI learningSuccessText;

    [Header("Settings")]
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        DestroyAfterLifetime();
    }

    public void SetLearningSuccessText(ProjectableObjectSO learnedObject)
    {
        learningSuccessText.text = $"{learnedObject.objectName} projection learned";
    }

    private void DestroyAfterLifetime() => Destroy(transform.parent.gameObject, lifeTime);
}
