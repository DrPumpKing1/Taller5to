using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneRing : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RectTransform center;
    [SerializeField] private float startAngle = 90f;
    [SerializeField] private float arcAngle = 360f;
    [SerializeField] private float radius = 100f;

    [Header("Render")]
    [SerializeField] private UILineRenderer lineRenderer;
    [SerializeField] private float thickness;
    [SerializeField] private RuneDot[] dots;

    [Header("Movement")]
    [SerializeField] private float rotation;

    private void Awake()
    {
        OrganizaPositions();
    }

    private void Start()
    {
        OrganizaPositions();
    }

    private void Update()
    {
        Rotation();
        OrganizaPositions();
    }

    private void OrganizaPositions()
    {
        if (dots == null) return;

        if(dots.Length == 0) return;

        int positionCount = dots.Length;

        lineRenderer.points = new Vector2[positionCount + 1];
        lineRenderer.thickness = thickness;

        float angleStep = arcAngle / positionCount;

        for (int i = 0; i < positionCount; i++)
        {
            RectTransform child = dots[i].GetComponent<RectTransform>();

            if(child == null) continue;

            float angle = startAngle + angleStep * i;

            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector2 position = new Vector2(x, y) * radius + center.anchoredPosition;

            child.anchoredPosition = position;
            lineRenderer.points[i] = position;
        }

        lineRenderer.points[positionCount] = lineRenderer.points[0];
        lineRenderer.SetAllDirty();
    }

    private void Rotation()
    {
        startAngle += rotation * Time.deltaTime;
    }
}
