using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SymbolCrafingUI : MonoBehaviour
{
    [Header("Compoents")]
    [SerializeField] private Component iRequiresSymbolCraftingComponent;

    [Header("UI Components")]
    [SerializeField] private Transform availableSymbolsContainer;
    [SerializeField] private Transform availableSymbolTemplate;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button closeButton;

    [Header("Settings")]
    [SerializeField] private SymbolCraftingSO symbolCraftingSO;

    public static event EventHandler OnAnySymbolCraftingUIOpen;
    public static event EventHandler OnAnySymbolCraftingUIClose;

    public IRequiresSymbolCrafting iRequiresSymbolCrafting;

    private void OnEnable()
    {
        OnAnySymbolCraftingUIOpen?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        OnAnySymbolCraftingUIClose?.Invoke(this, EventArgs.Empty);
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        availableSymbolTemplate.gameObject.SetActive(false);
    }

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseUI);
    }

    public void SetIRequiresSymbolCrafting(Component iRequiresSymbolCraftingComponent) 
    { 
        this.iRequiresSymbolCraftingComponent = iRequiresSymbolCraftingComponent;

        IRequiresSymbolCrafting interf = iRequiresSymbolCraftingComponent.GetComponent<IRequiresSymbolCrafting>();

        if (interf == null)
        {
            Debug.LogWarning("There's not a IRequiresSymbolCrafting attached to instantiated prefab");
            return;
        }

        iRequiresSymbolCrafting = interf;
    }

    private void SetUI(SymbolCraftingSO symbolCraftingSO)
    {
        this.symbolCraftingSO = symbolCraftingSO;

        SetTitleText(symbolCraftingSO.symbolToCraft.dialect);
        SetAvailableSymbolsUI(symbolCraftingSO.symbolToCraft.dialect);
    }

    private void SetTitleText(Dialect dialect) => titleText.text = $"Translate to dialect {dialect}";

    private void SetAvailableSymbolsUI(Dialect dialect)
    {
        foreach(DialectDictionary dialectDictionary in DictionaryManager.Instance.Dictionary)
        {
            if(dialectDictionary.dialect == dialect)
            {
                foreach (DialectSymbolSO dialectSymbolSO in dialectDictionary.dialectSymbolsSOs)
                {
                    GameObject availableSymbolUIGameObject = Instantiate(availableSymbolTemplate.gameObject, availableSymbolsContainer);
                    availableSymbolUIGameObject.SetActive(true);

                    AvailableSymbolSingleUI availableSymbolSingleUI = availableSymbolUIGameObject.GetComponent<AvailableSymbolSingleUI>();

                    if (!availableSymbolSingleUI)
                    {
                        Debug.LogWarning("There's not a AvailableSymbolSingleUI attached to instantiated prefab");
                        continue;
                    }

                    availableSymbolSingleUI.SetSymbolImage(dialectSymbolSO.symbolImage);
                }

                return;
            }
        }     
    }

    private void CloseUI()
    {
        Destroy(gameObject);
    }
}
