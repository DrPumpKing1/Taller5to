using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RectTransform container;
    [SerializeField] private float startAngle = 90f;
    [SerializeField] private float endAngle = 0f;
    [SerializeField] private float radius = 100f;

    private void Start()
    {
        if (container == null)
        {
            container = GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (container == null) return;

        OrganizaPositions();
    }

    private void OrganizaPositions()
    {
        int positionCount = container.childCount;

        if(positionCount == 0) return;

        float angleStep = (endAngle - startAngle) / positionCount;

        for(int i = 0; i < positionCount; i ++)
        {
            RectTransform child = container.GetChild(i).GetComponent<RectTransform>();

            float angle = startAngle + angleStep * (i + .5f);

            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            child.anchoredPosition = new Vector2(x, y) * radius + container.anchoredPosition;
        }
    }
}
