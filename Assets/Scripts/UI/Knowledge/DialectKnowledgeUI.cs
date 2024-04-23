using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialectKnowledgeUI : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private List<DialectKnowledgeSingleUI> dialectKnowledgeSingleUIs;

    private void OnEnable()
    {
        DialectManager.OnDialectKnowledgeSettingsCopied += KnowledgeManager_OnKnowledgeSettingsCopied;
        DialectManager.OnDialectKnowledgeChanged += KnowledgeManager_OnKnowledgeChanged;        
    }

    private void OnDisable()
    {
        DialectManager.OnDialectKnowledgeSettingsCopied -= KnowledgeManager_OnKnowledgeSettingsCopied;
        DialectManager.OnDialectKnowledgeChanged -= KnowledgeManager_OnKnowledgeChanged;
    }

    private void Start()
    {
        UpdateDialectLevels();
    }

    private void UpdateDialectLevels()
    {
        foreach (DialectKnowledge dialectKnowledge in DialectManager.Instance.DialectKnowledges)
        {
            foreach (DialectKnowledgeSingleUI dialectKnowledgeSingleUI in dialectKnowledgeSingleUIs)
            {
                if (dialectKnowledgeSingleUI.Dialect == dialectKnowledge.dialect)
                {
                    dialectKnowledgeSingleUI.SetLevelText(dialectKnowledge.level);
                    break;
                }
            }
        }
    }

    #region KnowledgeManager Subscriptions
    private void KnowledgeManager_OnKnowledgeSettingsCopied(object sender, EventArgs e)
    {
        UpdateDialectLevels();

    }
    private void KnowledgeManager_OnKnowledgeChanged(object sender, DialectManager.OnDialectKnowledgeChangedEventArgs e)
    {
        foreach (DialectKnowledgeSingleUI dialectKnowledgeSingleUI in dialectKnowledgeSingleUIs)
        {
            if (dialectKnowledgeSingleUI.Dialect != e.dialect) continue;

            dialectKnowledgeSingleUI.SetLevelText(e.newLevel);
        }
    }
    #endregion
}

