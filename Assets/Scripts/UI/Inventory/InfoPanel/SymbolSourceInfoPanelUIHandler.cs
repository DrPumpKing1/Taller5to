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

    private void UpdatePanel(DialectSymbolSourceSO dialectSymbolSourceSO)
    {
        SetSymbolSourceNameText(dialectSymbolSourceSO);
        SetSymbolSourceDescriptionText(dialectSymbolSourceSO);
        SetProvidedSymbols(dialectSymbolSourceSO);
    }

    private void SetSymbolSourceNameText(DialectSymbolSourceSO dialectSymbolSourceSO) => symbolSourceNameText.text = dialectSymbolSourceSO._name;
    private void SetSymbolSourceDescriptionText(DialectSymbolSourceSO dialectSymbolSourceSO) => symbolSourceDesciptionText.text = dialectSymbolSourceSO.description;
    private void SetProvidedSymbols(DialectSymbolSourceSO dialectSymbolSourceSO)
    {
        foreach (Transform child in symbolsProvidedContainer)
        {
            if (child == symbolProvidedTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (DialectSymbolSO dialectSymbolSO in dialectSymbolSourceSO.dialectSymbolSOs)
        {
            Transform symbolProvidedTranform = Instantiate(symbolProvidedTemplate, symbolsProvidedContainer);
            symbolProvidedTranform.gameObject.SetActive(true);

            symbolProvidedTranform.GetComponent<Image>().sprite = dialectSymbolSO.symbolImage;
        }
    }

    private void InfoPanelsUIHandler_OnShowSymbolSourceInfoPanelUI(object sender, InfoPanelsUIHandler.OnShowSymbolSourceInfoPanelUIEventArgs e)
    {
        UpdatePanel(e.dialectSymbolSourceSO);
        ShowPanel();
    }
}
