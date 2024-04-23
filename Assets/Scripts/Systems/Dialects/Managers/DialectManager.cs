using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialectManager : MonoBehaviour
{
    public static DialectManager Instance { get; private set; }

    [SerializeField] private List<DialectKnowledge> dialectKnowledges = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);
    [SerializeField] private DialectKnowledgeSettingsSO startingDialectKnowledgeSettingsSO;
    [SerializeField] private int maxDialectLevel = 5;

    public List<DialectKnowledge> DialectKnowledges { get { return dialectKnowledges; } }

    public static EventHandler<OnDialectKnowledgeChangedEventArgs> OnDialectKnowledgeChanged;
    public static EventHandler<OnDialectKnowledgeChangedEventArgs> OnDialectKnowledgeIncreased;
    public static EventHandler<OnDialectKnowledgeChangedEventArgs> OnDialectKnowledgeDecreased;

    public static EventHandler OnDialectKnowledgeSettingsCopied;

    public class OnDialectKnowledgeChangedEventArgs
    {
        public Dialect dialect;
        public int levelChange;
        public int newLevel;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        CopySettings(startingDialectKnowledgeSettingsSO);
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

    private void CopySettings(DialectKnowledgeSettingsSO knowledgeSettingsSO)
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

        OnDialectKnowledgeSettingsCopied?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeDialectKnowledge(Dialect dialect, int level)
    {
        if (level == 0f) return;

        DialectKnowledge dialectKnowledge = GetDialectKnowledgeByDialect(dialect);

        if (dialectKnowledge == null)
        {
            Debug.Log($"Dialect {dialect} not found, change will be ignored");
            return;
        }

        if (level > 0f) IncreaseDialectKnowledge(dialectKnowledge, level); 
        else DecreaseDialectKnowledge(dialectKnowledge, level);

        OnDialectKnowledgeChanged?.Invoke(this, new OnDialectKnowledgeChangedEventArgs { dialect = dialect, levelChange = level, newLevel = dialectKnowledge.level });
    }

    public void IncreaseDialectKnowledge(DialectKnowledge dialectKnowledge, int level)
    {
        int absoluteLevel = Mathf.Abs(level);

        dialectKnowledge.level = dialectKnowledge.level + absoluteLevel > maxDialectLevel ? maxDialectLevel : dialectKnowledge.level + absoluteLevel;

        OnDialectKnowledgeIncreased?.Invoke(this, new OnDialectKnowledgeChangedEventArgs { dialect = dialectKnowledge.dialect, levelChange = absoluteLevel, newLevel = dialectKnowledge.level });
    }

    public void DecreaseDialectKnowledge(DialectKnowledge dialectKnowledge, int level)
    {
        int absoluteLevel = Mathf.Abs(level);

        dialectKnowledge.level = dialectKnowledge.level - absoluteLevel <= 0 ? 0 : dialectKnowledge.level - absoluteLevel;

        OnDialectKnowledgeDecreased?.Invoke(this, new OnDialectKnowledgeChangedEventArgs { dialect = dialectKnowledge.dialect, levelChange = absoluteLevel, newLevel = dialectKnowledge.level });
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
