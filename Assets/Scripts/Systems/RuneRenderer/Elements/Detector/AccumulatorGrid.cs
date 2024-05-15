using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccumulatorGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private RectTransform canvas;
    [SerializeField] private float cellSize;
    public float CellSize { get { return cellSize; } }
    bool setup;

    [Header("Grid Properties")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector2 offset;

    [Header("Grid")]
    private Dictionary<Vector2, int> grid;

    [Header("Detection")]
    [SerializeField] private int minVotes;
    [SerializeField] private bool debug;
    [SerializeField] private GameObject dotPrefab;

    [Header("Rune Dots")]
    [SerializeField] private List<RuneDot> runeDots;

    private void Awake()
    {
        setup = false;
        InitializeGrid();
    }

    public void DetectPoints()
    {
        List<DrawSpot> points = new List<DrawSpot>(FindObjectsOfType<DrawSpot>());

        if (points.Count <= 0) return;

        foreach (DrawSpot point in points)
        {
            Vector2 nearestCell = GetNearestCell(point.Position);
            VoteCell(nearestCell);
        }
    }

    public void Results()
    {
        runeDots.ForEach(runeDot => runeDot.ListenResults());

        if (!debug) return;

        if(grid == null) return;

        foreach (KeyValuePair<Vector2, int> vote in grid)
        {
            if(vote.Value < minVotes) continue;
        
            GameObject spotGo = Instantiate(dotPrefab, (Vector3) vote.Key + new Vector3(Screen.width, Screen.height, 0f) / 2, Quaternion.identity);

            spotGo.transform.SetParent(canvas, true);

            spotGo.transform.localScale = spotGo.transform.localScale * cellSize * vote.Value / minVotes;
        }
    }

    public void ResetGrid()
    {
        grid.Clear();
        InitializeGrid();
    }

    public void ClearDots()
    {
        foreach(Transform child in canvas)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitializeGrid()
    {
        if(setup == false)
        {
            width = Mathf.FloorToInt(canvas.rect.width / cellSize);
            height = Mathf.FloorToInt(canvas.rect.height / cellSize);
            offset = Vector2.zero;
            offset = - new Vector2(canvas.rect.width, canvas.rect.height) / 2 - (Vector2)canvas.localPosition;
            //offset = (Vector2)canvas.position - new Vector2(canvas.rect.width, canvas.rect.height) ;

            grid = new Dictionary<Vector2, int>();
        }

        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int positionInt = new Vector2Int(x, y);
                Vector2 position =  new Vector2(positionInt.x, positionInt.y) * cellSize - offset;
                grid.Add(position, 0);
            }
        }

        setup = true;
    }

    public Vector2 GetNearestCell(Vector2 position)
    {
        int positionIntX = Mathf.FloorToInt((position.x + offset.x) / cellSize);
        int positionIntY = Mathf.FloorToInt((position.y + offset.y) / cellSize);

        return new Vector2(positionIntX, positionIntY) * cellSize - offset;
    }

    public void VoteCell(Vector2 position)
    {
        if (grid == null) return;

        if (!grid.ContainsKey(position)) return;

        grid[position]++;
    }

    public void AddRuneDot(RuneDot runeDot)
    {
        if(runeDots == null)
        {
            runeDots = new List<RuneDot>();
        }

        if(runeDots.Contains(runeDot)) return;

        if(runeDot == null) return;

        runeDots.Add(runeDot);
        runeDot.PopulateCells();
    }

    public bool ExistCell(Vector2 position)
    {
        return grid.ContainsKey(position);
    }

    public bool GetDetected(Vector2 position)
    {
        if(!grid.ContainsKey(position)) return false;

        return grid[position] >= minVotes;
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug) return;

        if (grid == null) return;

        foreach (KeyValuePair<Vector2, int> vote in grid)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(vote.Key, Vector3.one * cellSize);
        }
    }

}
