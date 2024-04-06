using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KnowledgeManager : MonoBehaviour
{
    public static KnowledgeManager Instance { get; private set; }

    [SerializeField] private List<DialectKnowledge> dialectKnowledge = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);

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

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one InteractionInput instance");
        }

        Instance = this;
    }

    public void ChangeKnowledge(Dialect dialect, float percentage)
    {
        if (percentage == 0f) return;

        DialectKnowledge dialectKnowledge = GetDialectKnowledgePercentageByDialect(dialect);

        if (dialectKnowledge == null)
        {
            Debug.Log($"Dialect {dialect} not found, addition will be ignored");
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

    private DialectKnowledge GetDialectKnowledgePercentageByDialect(Dialect dialect)
    {
        foreach (DialectKnowledge dialectKnowledgePercentage in dialectKnowledge)
        {
            if (dialectKnowledgePercentage.dialect == dialect) return dialectKnowledgePercentage;
        }

        return null;
    }
}
