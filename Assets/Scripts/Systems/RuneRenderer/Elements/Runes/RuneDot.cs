using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneDot : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AccumulatorGrid grid;
    [SerializeField] private bool isCenterDot = false;
    [SerializeField] private RectTransform center;
    public Vector2 localPoint;

    [Header("Render")]
    [SerializeField] private UILineRenderer lineRenderer;
    [SerializeField] private Image image;
    [SerializeField] private Color detectedColor;
    [SerializeField] private Color notDetectedColor;

    [Header("Detection")]
    [SerializeField] private int id;
    public int Id { get { return id; } }
    [SerializeField] RectTransform canvasRect;
    [SerializeField] RectTransform rect;
    [SerializeField] private int radius;
    [SerializeField] private bool detected;
    [SerializeField] private List<Vector2> cells;

    private void Awake()
    {
        detected = false;
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        grid.AddRuneDot(this);
    }

    private void Update()
    {
        RenderLine();
        RenderDetected();
        PopulateCells();
    }

    private void RenderLine()
    {
        if (center == null) return;

        if (isCenterDot) return;

        Vector2 position = gameObject.GetComponent<RectTransform>().anchoredPosition;

        lineRenderer.points = new Vector2[2];
        lineRenderer.points[0] = center.anchoredPosition - position;
        lineRenderer.points[1] = Vector2.zero;
        lineRenderer.SetAllDirty();
    }

    public bool GetDetected()
    {
        return detected;
    }

    private void RenderDetected()
    {
        image.color = detected ? detectedColor : notDetectedColor;
    }

    public void PopulateCells()
    {
        cells = new List<Vector2>();
        cells.Add(grid.GetNearestCell(localPoint + new Vector2(grid.Width * grid.CellSize, grid.Height * grid.CellSize) / 2));

        for(int i = 0; i < radius; i++) 
        {
            cells = AddNeighbourghood(cells, grid.CellSize);
        }
    }

    private List<Vector2> AddNeighbourghood(List<Vector2> cells, float cellSize)
    {
        List<Vector2> neighbors = new List<Vector2>();

        foreach (Vector2 cell in cells)
        {
            Vector2[] directions = new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(-1, 0)
            };

            foreach (Vector2 direction in directions)
            {
                Vector2 neighbor = cell + direction * cellSize;
                if (!grid.ExistCell(neighbor) && !neighbors.Contains(neighbor)) continue;

                neighbors.Add(neighbor);
            }
        }

        cells.ForEach(cell => neighbors.Add(cell));

        return neighbors;
    }

    public void ListenResults()
    {
        foreach(Vector2 cell in cells)
        {
            if(grid.GetDetected(cell))
            {
                detected = true;
                return;
            }
        }

        detected = false;
    }

    private void OnDrawGizmos()
    {
        if (cells == null) return;

        foreach (Vector2 cell in cells)
        {
            Gizmos.color = detected ? detectedColor : notDetectedColor;
            Gizmos.DrawCube(cell, Vector3.one * grid.CellSize);
        }
    }
}
