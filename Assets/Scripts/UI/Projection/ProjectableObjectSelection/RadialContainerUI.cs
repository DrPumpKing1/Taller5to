using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialContainerUI : MonoBehaviour
{
    [Header("Container Settings")]
    [SerializeField, Range(-360f,360f)] private float startAngle = 90f;
    [SerializeField, Range(-360f,360f)] private float endAngle = 0f;
    [SerializeField, Range(50f,400f)] private float radius = 100f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OrganizeChildPositions()
    {
        int positionCount = rectTransform.childCount;

        if (positionCount == 0) return;

        float angleStep = (endAngle - startAngle) / positionCount;

        for (int i = 0; i < positionCount; i++)
        {
            RectTransform child = rectTransform.GetChild(i).GetComponent<RectTransform>();

            float angle = startAngle + angleStep * (i + 0.5f);

            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            child.position = new Vector3(x, y ,0f) * radius + rectTransform.position;
        }
    }
}