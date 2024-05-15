using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UILineRenderer : Graphic
{
    public Vector2[] points;

    public float thickness;
    public bool center = true;

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();

        if (points == null) return;

        if (points.Length < 2) return;

        for (int i = 0; i < points.Length - 1; i++)
        {
            CreateLineSegment(points[i], points[i + 1], vertexHelper);

            int index = i * 5;

            vertexHelper.AddTriangle(index, index + 1, index + 3);
            vertexHelper.AddTriangle(index + 3, index + 2, index);

            if(i != 0)
            {
                vertexHelper.AddTriangle(index - 3, index - 1, index);
                vertexHelper.AddTriangle(index + 1, index - 1, index - 2);
            }
        }
    }

    private void CreateLineSegment(Vector3 pointStart, Vector3 pointEnd, VertexHelper vertexHelper)
    {
        Vector3 offset = center ? rectTransform.sizeDelta / 2 : Vector2.zero;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Quaternion pointStartRotation = Quaternion.Euler(0, 0, RotatePointTowards(pointStart, pointEnd) + 90);
        vertex.position = pointStartRotation * new Vector3(-thickness / 2, 0);
        vertex.position += pointStart - offset;
        vertexHelper.AddVert(vertex);
        vertex.position = pointStartRotation * new Vector3(thickness / 2, 0);
        vertex.position += pointStart - offset;
        vertexHelper.AddVert(vertex);

        Quaternion pointEndRotation = Quaternion.Euler(0, 0, RotatePointTowards(pointEnd, pointStart) - 90);
        vertex.position = pointEndRotation * new Vector3(-thickness / 2, 0);
        vertex.position += pointEnd - offset;
        vertexHelper.AddVert(vertex);
        vertex.position = pointEndRotation * new Vector3(thickness / 2, 0);
        vertex.position += pointEnd - offset;
        vertexHelper.AddVert(vertex);

        vertex.position = pointEnd - offset;
        vertexHelper.AddVert(vertex);
    }

    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI));
    }
}
