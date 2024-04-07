using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KnowledgeManager : MonoBehaviour
{
    public static KnowledgeManager Instance { get; private set; }

    [SerializeField] private List<DialectKnowledge> dialectKnowledges = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);
    [SerializeField] private KnowledgeSettingsSO startingKnowledgeSettingsSO;

    public EventHandler<OnKnowledgeChangedEventArgs> OnKnowledgeChanged;
    public EventHandler<OnKnowledgeChangedEventArgs> OnKnowledgeIncreased;
    public EventHandler<OnKnowledgeChangedEventArgs> OnKnowledgeDecreased;

    public class OnKnowledgeChangedEventArgs
    {
        public Dialect dialect;
        public float percentage;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        CopySettings(startingKnowledgeSettingsSO);
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one InteractionInput instance");
        }

        Instance = this;
    }

    private void CopySettings(KnowledgeSettingsSO knowledgeSettingsSO)
    {
        foreach (DialectKnowledge dialectKnowledgeSetting in knowledgeSettingsSO.dialectKnowledgeSettings)
        {
            foreach(DialectKnowledge dialectKnowledge in dialectKnowledges)
            {
                if(dialectKnowledge.dialect == dialectKnowledgeSetting.dialect)
                {
                    dialectKnowledge.percentage = dialectKnowledgeSetting.percentage;
                    break;
                }
            }
        }
    }

    public void ChangeKnowledge(Dialect dialect, float percentage)
    {
        if (percentage == 0f) return;

        DialectKnowledge dialectKnowledge = GetDialectKnowledgeByDialect(dialect);

        if (dialectKnowledge == null)
        {
            Debug.Log($"Dialect {dialect} not found, change will be ignored");
            return;
        }

        if (percentage > 0f) IncreaseKnowledge(dialectKnowledge, percentage); 
        else DecreaseKnowledge(dialectKnowledge, percentage);

        OnKnowledgeChanged?.Invoke(this, new OnKnowledgeChangedEventArgs { dialect = dialect, percentage = percentage });
    }

    public void IncreaseKnowledge(DialectKnowledge dialectKnowledge, float percentage)
    {
        float absolutePercentage = Mathf.Abs(percentage);

        dialectKnowledge.percentage = dialectKnowledge.percentage + absolutePercentage > 1f? 1f: dialectKnowledge.percentage + absolutePercentage;
        dialectKnowledge.percentage = GeneralMethods.RoundTo2DecimalPlaces(dialectKnowledge.percentage);

        OnKnowledgeIncreased?.Invoke(this, new OnKnowledgeChangedEventArgs { dialect = dialectKnowledge.dialect, percentage = absolutePercentage });
    }

    public void DecreaseKnowledge(DialectKnowledge dialectKnowledge, float percentage)
    {
        float absolutePercentage = Mathf.Abs(percentage);

        dialectKnowledge.percentage = dialectKnowledge.percentage - absolutePercentage <= 0f ? 0f : dialectKnowledge.percentage - absolutePercentage;
        dialectKnowledge.percentage = GeneralMethods.RoundTo2DecimalPlaces(dialectKnowledge.percentage);

        OnKnowledgeDecreased?.Invoke(this, new OnKnowledgeChangedEventArgs { dialect = dialectKnowledge.dialect, percentage = absolutePercentage });
    }

    private DialectKnowledge GetDialectKnowledgeByDialect(Dialect dialect)
    {
        foreach (DialectKnowledge dialectKnowledge in dialectKnowledges)
        {
            if (dialectKnowledge.dialect == dialect) return dialectKnowledge;
        }

        return null;
    }
}
