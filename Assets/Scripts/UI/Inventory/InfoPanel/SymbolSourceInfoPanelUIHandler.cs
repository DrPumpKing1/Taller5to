using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SymbolSourceInfoPanelUIHandler : InfoPanelUIHandler
{
    [Header("UIComponents")]
    [SerializeField] private TextMeshProUGUI symbolSourceNameText;
    [SerializeField] private TextMeshProUGUI symbolSourceDesciptionText;
    [Space]
    [SerializeField] private Transform symbolsProvidedContainer;
    [SerializeField] private Transform symbolProvidedTemplate;

    protected override void OnEnable()
    {
        base.OnEnable();
        infoPanelsUIHandler.OnShowSymbolSourceInfoPanelUI += InfoPanelsUIHandler_OnShowSymbolSourceInfoPanelUI;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        infoPanelsUIHandler.OnShowSymbolSourceInfoPanelUI -= InfoPanelsUIHandler_OnShowSymbolSourceInfoPanelUI;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        symbolProvidedTemplate.gameObject.SetActive(false);
    }

    private void UpdatePanel(SymbolSourceSO symbolSourceSO)
    {
        SetSymbolSourceNameText(symbolSourceSO);
        SetSymbolSourceDescriptionText(symbolSourceSO);
        SetProvidedSymbols(symbolSourceSO);
    }

    private void SetSymbolSourceNameText(SymbolSourceSO symbolSourceSO) => symbolSourceNameText.text = symbolSourceSO._name;
    private void SetSymbolSourceDescriptionText(SymbolSourceSO symbolSourceSO) => symbolSourceDesciptionText.text = symbolSourceSO.description;
    private void SetProvidedSymbols(SymbolSourceSO symbolSourceSO)
    {
        foreach (Transform child in symbolsProvidedContainer)
        {
            if (child == symbolProvidedTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (SymbolSO symbolSO in symbolSourceSO.symbolSOs)
        {
            Transform symbolProvidedTranform = Instantiate(symbolProvidedTemplate, symbolsProvidedContainer);
            symbolProvidedTranform.gameObject.SetActive(true);

            symbolProvidedTranform.GetComponent<Image>().sprite = symbolSO.symbolImage;
        }
    }

    private void InfoPanelsUIHandler_OnShowSymbolSourceInfoPanelUI(object sender, InfoPanelsUIHandler.OnShowSymbolSourceInfoPanelUIEventArgs e)
    {
        UpdatePanel(e.symbolSourceSO);
        ShowPanel();
    }
}
