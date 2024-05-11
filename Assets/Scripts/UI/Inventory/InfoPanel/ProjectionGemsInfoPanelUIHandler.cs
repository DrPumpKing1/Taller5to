using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ProjectionGemsInfoPanelUIHandler : InfoPanelUIHandler
{
    [Header("UIComponents")]
    [SerializeField] private TextMeshProUGUI availableProjectionGemsText;

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

    private void SetAvailableProjectionGemsText(int totalProjectionGems, int availableProjectionGems) => availableProjectionGemsText.text = $"Available Projection Gems\n{availableProjectionGems}/{totalProjectionGems}";

    private void InfoPanelsUIHandler_OnShowProjectionGemsInfoPanelUI(object sender, InfoPanelsUIHandler.OnShowProjectionGemsInfoPanelUIEventArgs e)
    {
        SetAvailableProjectionGemsText(e.totalProjectionGems, e.availableProjectionGems);
        ShowPanel();
    }
}
