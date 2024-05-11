using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ProjectionGemsInfoPanelUIHandler : InfoPanelUIHandler
{
    [Header("UIComponents")]
    [SerializeField] private TextMeshProUGUI projectionGemsTitleText;
    [SerializeField] private TextMeshProUGUI projectionGemsDescriptionText;
    [SerializeField] private TextMeshProUGUI availableProjectionGemsText;

    [Header("Texts")]
    [SerializeField] private string projectionGemsTitle;
    [SerializeField, TextArea(3,10)] private string projectionGemsDescription;

    protected override void OnEnable()
    {
        base.OnEnable();
        infoPanelsUIHandler.OnShowProjectionGemsInfoPanelUI += InfoPanelsUIHandler_OnShowProjectionGemsInfoPanelUI; ;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        infoPanelsUIHandler.OnShowProjectionGemsInfoPanelUI -= InfoPanelsUIHandler_OnShowProjectionGemsInfoPanelUI;
    }

    private void SetProjectionGemsTitleText() => projectionGemsTitleText.text = projectionGemsTitle;
    private void SetProjectionGemsDescriptionText() => projectionGemsDescriptionText.text = projectionGemsDescription;
    private void SetAvailableProjectionGemsText(int totalProjectionGems, int availableProjectionGems) => availableProjectionGemsText.text = $"Available Projection Gems\n{availableProjectionGems}/{totalProjectionGems}";

    private void InfoPanelsUIHandler_OnShowProjectionGemsInfoPanelUI(object sender, InfoPanelsUIHandler.OnShowProjectionGemsInfoPanelUIEventArgs e)
    {
        SetProjectionGemsTitleText();
        SetProjectionGemsDescriptionText();
        SetAvailableProjectionGemsText(e.totalProjectionGems, e.availableProjectionGems);

        ShowPanel();
    }
}
