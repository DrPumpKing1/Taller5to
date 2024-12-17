using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InspectionPrefabHolderHandler inspectionPrefabHolderHandler;
    [SerializeField] private Camera inspectionCamera;

    private Transform currentInspectionPrefab;

    public InspectionPrefabHolderHandler InspectionPrefabHolderHandler => inspectionPrefabHolderHandler;

    private void OnEnable()
    {
        InspectableJournalInfoPopUpHandler.OnInspectionUIOpen += InspectableJournalInfoPopUpHandler_OnInspectionUIOpen;
    }

    private void OnDisable()
    {
        InspectableJournalInfoPopUpHandler.OnInspectionUIOpen -= InspectableJournalInfoPopUpHandler_OnInspectionUIOpen;
    }

    private void Start()
    {
        ClearCurrentInspectionPrefab();
    }

    private void ResetDragHolderRotation()
    {
        inspectionPrefabHolderHandler.ResetDragHolderRotation();
    }

    private void SetBackgroundColor(Color backgroundColor)
    {
        inspectionCamera.backgroundColor = backgroundColor;
    }

    private void HandleInspectionPrefab(Transform prefab)
    {
        ResetDragHolderRotation();

        if(currentInspectionPrefab != null)
        {
            Destroy(currentInspectionPrefab.gameObject);
            ClearCurrentInspectionPrefab();
        }

        Transform inspectionPrefab = Instantiate(prefab, inspectionPrefabHolderHandler.DragHolder);
        SetCurrentInspectionPrefab(inspectionPrefab);
    }

    private void SetCurrentInspectionPrefab(Transform prefab) => currentInspectionPrefab = prefab;

    private void ClearCurrentInspectionPrefab() => currentInspectionPrefab = null;

    #region InspectableJournalInfoPopUpHandler Subscriptions
    private void InspectableJournalInfoPopUpHandler_OnInspectionUIOpen(object sender, InspectableJournalInfoPopUpHandler.OnInspectionUIOpenEventArgs e)
    {
        SetBackgroundColor(e.inspectBackgroundColor);
        HandleInspectionPrefab(e.inspectionPrefab);
    }
    #endregion
}
