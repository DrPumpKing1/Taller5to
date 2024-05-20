using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static AccumulatorGrid;
using System.Linq;

public class SymbolCraftingSingleUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SymbolCraftingSO symbolCraftingSO;
    [SerializeField] private AccumulatorGrid grid;

    [Header("UI Components")]
    [SerializeField] private Image imageToTranslate;

    [Header("Settings")]
    [SerializeField] private bool isCrafted;

    public bool IsCrafted { get { return isCrafted; } }

    public event EventHandler OnSymbolCrafted;

    private void OnEnable()
    {
        grid.DetetectionHandler += OnSymbolCraftedHandler;
    }

    private void OnDisable()
    {
        grid.DetetectionHandler -= OnSymbolCraftedHandler;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        isCrafted = false;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O)) 
        //{
        //    if (transform.parent.parent.parent.GetComponent<CanvasGroup>().alpha != 1) return;
        //    CraftSymbol(); //TestToCraftSymbol
        //}

    }
    
    #region SetUp Methods
    public void SetSymbolCraftingSingleUI(SymbolCraftingSO symbolCraftingSO)
    {
        this.symbolCraftingSO = symbolCraftingSO;
        SetImageToTranslate(symbolCraftingSO.imageToTranslateSprite);
    }

    private void SetImageToTranslate(Sprite sprite) => imageToTranslate.sprite = sprite;
    #endregion

    //Handle DrawingPointsLogic
    private void CraftSymbol()
    {
        if (isCrafted) return;

        isCrafted = true;
        OnSymbolCrafted?.Invoke(this, EventArgs.Empty);
    }

    private void OnSymbolCraftedHandler(object sender, DetectionHandlerArgs e)
    {
        var ids = e.detectedIds;
        var idsToCraft = symbolCraftingSO.pointsIDs;

        var substractList = ids.Except(idsToCraft).ToList();
        var substractList2 = idsToCraft.Except(ids).ToList();

        if(substractList.Count == 0 && substractList2.Count == 0)
        {
            CraftSymbol();
        }
    }
}
