using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialectKnowledgeSingleUI : MonoBehaviour
{
    [Header("Dialect Settings")]
    [SerializeField] private Dialect dialect;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI dialectText;
    [SerializeField] private TextMeshProUGUI levetText;

    public Dialect Dialect { get { return dialect; } }

    private void Start()
    {
        SetDialectText();
    }

    public void SetDialectText() => dialectText.text = $"Dialecto {dialect}";

    public void SetLevelText(int level) => levetText.text = $"Nivel {level}";
}
