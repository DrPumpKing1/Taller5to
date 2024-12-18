using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectionUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform prefabHolder;
    [SerializeField] private InspectionUIDragHandler inspectionUIDragHandler;
    [SerializeField] private Camera inspectionCamera;
    [SerializeField] private Light inspectionLight;
    [SerializeField] private Image inspectionBG;

    private Transform currentInspectionPrefab;

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


    private void SetBackgroundColor(Color backgroundColor)
    {
        inspectionCamera.backgroundColor = backgroundColor;
    }

    private void SetLightColor(Color lightColor)
    {
        inspectionLight.color = lightColor;
    }

    private void SetLightIntensity(float intensity)
    {
        inspectionLight.intensity = intensity;
    }

    private void SetDefaultInspectionAngles(Vector2 angles)
    {
        inspectionUIDragHandler.SetDefaultAngles(angles);
    }

    private void SetInspectionBackground(Sprite backgroundSprite)
    {
        inspectionBG.sprite = backgroundSprite;
    }

    private void HandleInspectionPrefab(Transform prefab)
    {
        inspectionUIDragHandler.ResetDragHolderRotationInmediately();

        if(currentInspectionPrefab != null)
        {
            Destroy(currentInspectionPrefab.gameObject);
            ClearCurrentInspectionPrefab();
        }

        Transform inspectionPrefab = Instantiate(prefab, prefabHolder);
        SetCurrentInspectionPrefab(inspectionPrefab);
    }


    private void SetCurrentInspectionPrefab(Transform prefab) => currentInspectionPrefab = prefab;

    private void ClearCurrentInspectionPrefab() => currentInspectionPrefab = null;

    #region InspectableJournalInfoPopUpHandler Subscriptions
    private void InspectableJournalInfoPopUpHandler_OnInspectionUIOpen(object sender, InspectableJournalInfoPopUpHandler.OnInspectionUIOpenEventArgs e)
    {
        SetInspectionBackground(e.inspectionBGSprite);
        SetBackgroundColor(e.inspectBackgroundColor);
        SetLightColor(e.lightColor);
        SetLightIntensity(e.lightIntensity);
        SetDefaultInspectionAngles(e.inspectionDefaultAngles);
        HandleInspectionPrefab(e.inspectionPrefab);
    }
    #endregion
}
