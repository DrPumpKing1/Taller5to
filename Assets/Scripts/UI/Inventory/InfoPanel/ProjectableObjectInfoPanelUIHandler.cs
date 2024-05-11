using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectableObjectInfoPanelUIHandler : InfoPanelUIHandler
{
    [Header("UIComponents")]
    [SerializeField] private TextMeshProUGUI projectableObjectNameText;
    [SerializeField] private TextMeshProUGUI projectableObjectDescriptionText;
    [SerializeField] private TextMeshProUGUI projectableObjectCostText;

    protected override void OnEnable()
    {
        base.OnEnable();
        infoPanelsUIHandler.OnShowProjectableObjectInfoPanelUI += InfoPanelsUIHandler_OnOpenProjectableObjectInfoPanelUI;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        infoPanelsUIHandler.OnShowProjectableObjectInfoPanelUI -= InfoPanelsUIHandler_OnOpenProjectableObjectInfoPanelUI;
    }

    private void SetProjectableObjectNameText(ProjectableObjectSO projectableObjectSO) => projectableObjectNameText.text = projectableObjectSO.objectName;
    private void SetProjectableObjectDescriptionText(ProjectableObjectSO projectableObjectSO) => projectableObjectDescriptionText.text = projectableObjectSO.description;
    private void SetProjectableObjectCostText(ProjectableObjectSO projectableObjectSO) => projectableObjectCostText.text = $"Cost: {projectableObjectSO.projectionGemsCost} Projection Gems";
    
    private void InfoPanelsUIHandler_OnOpenProjectableObjectInfoPanelUI(object sender, InfoPanelsUIHandler.OnShowProjectableObjectInfoPanelUIEventArgs e)
    {
        SetProjectableObjectNameText(e.projectableObjectSO);
        SetProjectableObjectDescriptionText(e.projectableObjectSO);
        SetProjectableObjectCostText(e.projectableObjectSO);

        ShowPanel();
    }
}
