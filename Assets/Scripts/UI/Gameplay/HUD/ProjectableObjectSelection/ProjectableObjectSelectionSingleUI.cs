using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectableObjectSelectionSingleUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image projectableObjectImage;
    [SerializeField] private Image border;
    [SerializeField] private Image keybindImage;

    [Header("Colors")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;

    [SerializeField] private bool isSelected;

    public int LinkedIndex { get; private set; }

    public bool IsSelected { get { return isSelected; } }

    public void SetProjectableObjectImage(Sprite sprite) => projectableObjectImage.sprite = sprite;
    public void SetProjectableObjectKeybindImage(Sprite keybindSprite) => keybindImage.sprite = keybindSprite;

    public void SelectUI()
    {
        HighlightBorder();
        isSelected = true;
    }

    public void DeselectUI()
    {
        UnHighlightBorder();
        isSelected = false;
    }

    private void HighlightBorder() => border.color = selectedColor;
    private void UnHighlightBorder() => border.color = deselectedColor;

    public void SetLinkedIndex(int index) => LinkedIndex = index;
}
