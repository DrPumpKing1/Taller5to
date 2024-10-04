using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LearningSuccessUI : FeedbackUI
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI learningSuccessText;

    public void SetLearningSuccessText(ProjectableObjectSO learnedObject)
    {
        learningSuccessText.text = $"{learnedObject.objectName} projection learned";
    }
}
