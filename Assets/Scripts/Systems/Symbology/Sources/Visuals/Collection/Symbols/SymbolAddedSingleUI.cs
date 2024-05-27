using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SymbolAddedSingleUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image symbolImage;

    public void SetSymbolImage(Sprite sprite) => symbolImage.sprite = sprite;
}
