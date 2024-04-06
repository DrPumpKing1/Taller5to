using UnityEngine;

[System.Serializable]
public class DialectKnowledge
{
    public Dialect dialect;
    [Range(0, 1f)] public float percentage;
}
