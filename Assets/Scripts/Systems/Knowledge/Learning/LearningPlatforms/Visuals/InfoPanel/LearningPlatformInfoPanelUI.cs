using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPlatformInfoPanelUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifeTime;

    public float LifeTime { get { return lifeTime; } }

    private void Awake()
    {
        //DestroyAfterLifetime();
    }

    private void DestroyAfterLifetime() => Destroy(gameObject, lifeTime);

    public void SetInfoPanelContents(ProjectableObjectSO projectableObjectToLearn, List<DialectKnowledge> dialectKnowledgeRequirements)
    {

    }

    public void DestroyPanel() => Destroy(gameObject);
}
