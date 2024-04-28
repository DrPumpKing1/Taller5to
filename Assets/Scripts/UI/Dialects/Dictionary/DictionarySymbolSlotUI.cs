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
    [SerializeField] private int symbolID;

    public int SymbolID { get { return symbolID; } }

    public void SetSymbolImage(Sprite sprite) => symbolImage.sprite = sprite;
    public void SetMeaningImage(Sprite sprite) => meaningImage.sprite = sprite;
}
