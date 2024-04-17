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

    public void SetKnowledgeAddedText(ProjectableObjectSO learnedObject)
    {
        learningSuccessText.text = $"Has aprendido a proyectar el objeto {learnedObject.objectName}";
    }

    private void DestroyAfterLifetime() => Destroy(gameObject, lifeTime);
}
