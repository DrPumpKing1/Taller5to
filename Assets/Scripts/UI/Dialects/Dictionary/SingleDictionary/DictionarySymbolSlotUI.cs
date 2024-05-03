using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionarySymbolSlotUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image symbolImage;
    [SerializeField] private Image meaningImage;

    [Header("Settings")]
    [SerializeField] private DialectSymbolSO dialectSymbolSO;

    public DialectSymbolSO DialectSymbolSO { get { return dialectSymbolSO; } }

    public void ShowSymbol()
    {
        ShowSymbolImage();
        ShowMeaningImage();
    }

    private void ShowSymbolImage() => symbolImage.sprite = dialectSymbolSO.symbolImage;
    private void ShowMeaningImage() => meaningImage.sprite = dialectSymbolSO.meaningImage;
}
