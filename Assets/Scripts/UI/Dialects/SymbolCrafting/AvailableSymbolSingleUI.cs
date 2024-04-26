using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableSymbolSingleUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image symbolImage;

    public void SetSymbolImage(Sprite sprite) => symbolImage.sprite = sprite;
}
