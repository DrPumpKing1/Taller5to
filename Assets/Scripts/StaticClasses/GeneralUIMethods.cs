using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public static class GeneralUIMethods 
{
    public static void SetCanvasGroupAlpha(CanvasGroup canvasGroup, float alpha) => canvasGroup.alpha = alpha;

    public static void SetImageFillRatio(Image image, float fillRatio) => image.fillAmount = fillRatio;
}
