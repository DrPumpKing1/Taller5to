using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPlatformShowInfoVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LearningPlatformShowInfo learningPlatformShowInfo;

    [Header("Learning Platfown Show Info Settings")]
    [SerializeField] private Transform infoPanelUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private GameObject learningPlatformInfoPanelUIGameObject;

    private void OnEnable()
    {
        learningPlatformShowInfo.OnShowInfo += LearningPlatformShowInfo_OnShowInfo;
        learningPlatformShowInfo.OnHideInfo += LearningPlatformShowInfo_OnHideInfo;
    }

    private void OnDisable()
    {
        learningPlatformShowInfo.OnShowInfo -= LearningPlatformShowInfo_OnShowInfo;
        learningPlatformShowInfo.OnHideInfo -= LearningPlatformShowInfo_OnHideInfo;
    }

    #region LearningPlatformShowInfo Event Subscriptions
    private void LearningPlatformShowInfo_OnShowInfo(object sender, LearningPlatformShowInfo.OnLearningPlatformInfoEventArgs e)
    {
        if (learningPlatformInfoPanelUIGameObject) return;

        learningPlatformInfoPanelUIGameObject = Instantiate(infoPanelUIPrefab.gameObject, transform.position + instantiationPositionOffset, transform.rotation);

        LearningPlatformInfoPanelUI learningPlatformInfoPanelUI = learningPlatformInfoPanelUIGameObject.GetComponentInChildren<LearningPlatformInfoPanelUI>();

        if (!learningPlatformInfoPanelUI)
        {
            Debug.LogWarning("There's not a LearningPlatformInfoPanelUI attached to instantiated prefab");
            return;
        }

        learningPlatformInfoPanelUI.SetInfoPanelContents(e.projectableObjectToLearn, e.dialectKnowledgeRequirements);
    }

    private void LearningPlatformShowInfo_OnHideInfo(object sender, LearningPlatformShowInfo.OnLearningPlatformInfoEventArgs e)
    {
        if (!learningPlatformInfoPanelUIGameObject) return;

        LearningPlatformInfoPanelUI learningPlatformInfoPanelUI = learningPlatformInfoPanelUIGameObject.GetComponentInChildren<LearningPlatformInfoPanelUI>();

        if (!learningPlatformInfoPanelUI)
        {
            Debug.LogWarning("There's not a LearningPlatformInfoPanelUI attached to instantiated prefab");
            return;
        }

        learningPlatformInfoPanelUI.DestroyPanel();
    }
    #endregion

}