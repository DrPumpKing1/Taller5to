using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Components")]
    [SerializeField] private GameObject spotPrefab;

    [Header("State")]
    [SerializeField] private bool drawing;
    [SerializeField] private bool overCanvas;
    [SerializeField] private bool clearCanvas;
    private DrawSpot previousDrawSpot;

    [Header("Brush Configuration")]
    [SerializeField] private float spotSize;
    [SerializeField] private Texture2D cursorBrush;
    [SerializeField] private float drawInterval;
    private float timer;

    [Header("Events")]
    [SerializeField] public UnityEvent OnDrawStart;
    [SerializeField] public UnityEvent OnDrawEnd;

    private void Start()
    {
        clearCanvas = true;
    }

    public void SetPaintCursor()
    {
        Cursor.SetCursor(cursorBrush, Vector2.zero, CursorMode.Auto);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if(timer >= 0 && drawing) timer -= Time.deltaTime;

        if (drawing && overCanvas)
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Draw(mousePos);
        }
    }

    private void Draw(Vector2 position)
    {
        if (timer >= 0) return;
        timer = drawInterval;

        RectTransform rectTransform = GetComponent<RectTransform>();

        GameObject spotObj = Instantiate(spotPrefab, transform);
        RectTransform rect = spotObj.GetComponent<RectTransform>();

        rect.position = position;

        if (previousDrawSpot != null && rect != null)
        {
            Vector3 dif = position - previousDrawSpot.Position;

            if(dif.magnitude > 0)
            {
                rect.sizeDelta = new Vector3(spotSize, Mathf.Max(dif.magnitude, spotSize));
                rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI + 90));
            }
        }

        DrawSpot spot = spotObj.GetComponent<DrawSpot>();
        spot?.SetSpotData(position);

        previousDrawSpot = spot;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(clearCanvas && OnDrawStart != null) OnDrawStart.Invoke();
            drawing = true;
            clearCanvas = false;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            EraseCanvas();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        overCanvas = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overCanvas = false;

        if (drawing) EraseCanvas();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            drawing = false;
            previousDrawSpot = null;
        }
    }

    public void EraseCanvas()
    {
        drawing = false;
        clearCanvas = true;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (OnDrawEnd != null) OnDrawEnd.Invoke();
    }
}
