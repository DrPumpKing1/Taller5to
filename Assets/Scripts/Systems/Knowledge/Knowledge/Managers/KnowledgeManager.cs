using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KnowledgeManager : MonoBehaviour
{
    public static KnowledgeManager Instance { get; private set; }

    [SerializeField] private List<DialectKnowledge> dialectKnowledges = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);
    [SerializeField] private KnowledgeSettingsSO startingKnowledgeSettingsSO;
    [SerializeField] private int maxDialectLevel = 10;

    public static EventHandler<OnKnowledgeChangedEventArgs> OnKnowledgeChanged;
    public static EventHandler<OnKnowledgeChangedEventArgs> OnKnowledgeIncreased;
    public static EventHandler<OnKnowledgeChangedEventArgs> OnKnowledgeDecreased;

    public class OnKnowledgeChangedEventArgs
    {
        public Dialect dialect;
        public float level;
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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one KnowledgeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void CopySettings(KnowledgeSettingsSO knowledgeSettingsSO)
    {
        foreach (DialectKnowledge dialectKnowledgeSetting in knowledgeSettingsSO.dialectKnowledgeSettings)
        {
            foreach(DialectKnowledge dialectKnowledge in dialectKnowledges)
            {
                if(dialectKnowledge.dialect == dialectKnowledgeSetting.dialect)
                {
                    dialectKnowledge.level = dialectKnowledgeSetting.level;
                    break;
                }
            }
        }
    }

    public void ChangeKnowledge(Dialect dialect, int level)
    {
        if (level == 0f) return;

        DialectKnowledge dialectKnowledge = GetDialectKnowledgeByDialect(dialect);

        if (dialectKnowledge == null)
        {
            Debug.Log($"Dialect {dialect} not found, change will be ignored");
            return;
        }

        if (level > 0f) IncreaseKnowledge(dialectKnowledge, level); 
        else DecreaseKnowledge(dialectKnowledge, level);

        OnKnowledgeChanged?.Invoke(this, new OnKnowledgeChangedEventArgs { dialect = dialect, level = level });
    }

    public void IncreaseKnowledge(DialectKnowledge dialectKnowledge, int level)
    {
        int absoluteLevel = Mathf.Abs(level);

        dialectKnowledge.level = dialectKnowledge.level + absoluteLevel > maxDialectLevel ? maxDialectLevel : dialectKnowledge.level + absoluteLevel;

        OnKnowledgeIncreased?.Invoke(this, new OnKnowledgeChangedEventArgs { dialect = dialectKnowledge.dialect, level = absoluteLevel });
    }

    public void DecreaseKnowledge(DialectKnowledge dialectKnowledge, int level)
    {
        int absoluteLevel = Mathf.Abs(level);

        dialectKnowledge.level = dialectKnowledge.level - absoluteLevel <= 0 ? 0 : dialectKnowledge.level - absoluteLevel;

        OnKnowledgeDecreased?.Invoke(this, new OnKnowledgeChangedEventArgs { dialect = dialectKnowledge.dialect, level = absoluteLevel });
    }

    private DialectKnowledge GetDialectKnowledgeByDialect(Dialect dialect)
    {
        foreach (DialectKnowledge dialectKnowledge in dialectKnowledges)
        {
            if (dialectKnowledge.dialect == dialect) return dialectKnowledge;
        }

        return null;
    }

    public List<DialectKnowledge> GetDialectKnowledges() => dialectKnowledges;
}
