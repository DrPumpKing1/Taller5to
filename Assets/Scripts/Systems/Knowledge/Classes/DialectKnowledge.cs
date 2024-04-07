using UnityEngine;

[System.Serializable]
public class DialectKnowledge
{
    public Dialect dialect;
    [Range(0, 10)] public int level;
}
