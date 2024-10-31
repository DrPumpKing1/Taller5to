using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectableObjectDematerializationVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectableObjectSO projectableObjectSO;
    [SerializeField] private ProjectableObjectDematerialization projectableObjectDematerialization;
    [SerializeField] private ProjectableObjectRotation projectableObjectRotation;
    [SerializeField] private VisualEffect projectableObjectDematerializationVFX;
    [Space]
    [SerializeField] private List<ProjectionVFXSetting> projectionVFXSettings;

    [Header("Settings")]
    [SerializeField, Range(1f,3f)] private float lifespan;

    private const string BOX_CENTER_PROPERTY = "BoxCenter";
    private const string BOX_SIZE_PROPERTY = "BoxSize";

    private bool hasUnsubscribed;

    [Serializable]
    public class ProjectionVFXSetting
    {
        public ProjectableObjectSO projectableObjectSO;
        public Vector3 boxOffset;
        public Vector3 boxSize;
        public bool rectifyDueToOrientation;
    }

    private void OnEnable()
    {
        projectableObjectDematerialization.OnStartDematerialization += ProjectableObjectDematerialization_OnStartDematerialization;
        projectableObjectDematerialization.OnEndDematerialization += ProjectableObjectDematerialization_OnEndDematerialization;
        projectableObjectDematerialization.OnObjectDematerialized += ProjectableObjectDematerialization_OnObjectDematerialized;

        ProjectionResetObject.OnAnyStartProjectionResetObjectUse += ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse += ProjectionResetObject_OnAnyEndProjectionResetObjectUse;
    }

    private void OnDisable()
    {
        UnsubscribeFromProjectableObjectDematerializationEvents();

        ProjectionResetObject.OnAnyStartProjectionResetObjectUse -= ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse -= ProjectionResetObject_OnAnyEndProjectionResetObjectUse;
    }

    private void Start()
    {
        InitializeVariables();
        InitializeVFX();
    }

    private void InitializeVariables()
    {
        hasUnsubscribed = false;
    }

    private void InitializeVFX()
    {
        projectableObjectDematerializationVFX.gameObject.SetActive(true);
        projectableObjectDematerializationVFX.Stop();
    }

    private void SetVFXBoxCenter(Vector3 center)
    {
        if (projectableObjectDematerializationVFX.HasVector3(BOX_CENTER_PROPERTY))
        {
            projectableObjectDematerializationVFX.SetVector3(BOX_CENTER_PROPERTY, center);
        }
    }

    private void SetVFXBoxSize(Vector3 size)
    {
        if (projectableObjectDematerializationVFX.HasVector3(BOX_SIZE_PROPERTY))
        {
            projectableObjectDematerializationVFX.SetVector3(BOX_SIZE_PROPERTY, size);
        }
    }

    private ProjectionVFXSetting GetVFXSettingsByProjectableObjectSO(ProjectableObjectSO projectableObjectSO)
    {
        foreach (ProjectionVFXSetting projectionVFXSetting in projectionVFXSettings)
        {
            if (projectableObjectSO == projectionVFXSetting.projectableObjectSO) return projectionVFXSetting;
        }

        return null;
    }

    private void StartVFX()
    {
        ProjectionVFXSetting projectionVFXSetting = GetVFXSettingsByProjectableObjectSO(projectableObjectSO);

        if (projectionVFXSetting == null)
        {
            Debug.Log("DematerializationVFX Setting not found for ProjectableObjectSelected, VFX will be ignored");
            return;
        }

        Vector3 boxCenter  = projectionVFXSetting.boxOffset;
        Vector3 boxSize = projectionVFXSetting.boxSize;

        if (projectionVFXSetting.rectifyDueToOrientation) boxSize = RectifyBoxSizeDueToOrientation(boxSize);

        SetVFXBoxCenter(boxCenter);
        SetVFXBoxSize(boxSize);

        projectableObjectDematerializationVFX.Play();
    }

    private Vector3 RectifyBoxSizeDueToOrientation(Vector3 boxSize)
    {
        if (!projectableObjectRotation) return boxSize;

        Vector2 orientation = GeneralMethods.Vector3ToVector2(projectableObjectRotation.DesiredDirection);

        if (orientation == Vector2.zero) return boxSize;
        if (orientation.x == orientation.y) return boxSize;

        if (Math.Abs(orientation.x) > Math.Abs(orientation.y)) return boxSize;

        if (Math.Abs(orientation.x) < Math.Abs(orientation.y))
        {
            Vector3 rectifiedBoxSize = new Vector3(boxSize.z, boxSize.y, boxSize.x);
            return rectifiedBoxSize;
        }

        return boxSize;
    }

    private void StopVFX() => projectableObjectDematerializationVFX.Stop();

    private void DetachVFX() => transform.parent = null;

    private void UnsubscribeFromProjectableObjectDematerializationEvents()
    {
        if (hasUnsubscribed) return;

        projectableObjectDematerialization.OnStartDematerialization -= ProjectableObjectDematerialization_OnStartDematerialization;
        projectableObjectDematerialization.OnEndDematerialization -= ProjectableObjectDematerialization_OnEndDematerialization;
        projectableObjectDematerialization.OnObjectDematerialized -= ProjectableObjectDematerialization_OnObjectDematerialized;

        hasUnsubscribed = true;
    }

    private void DestroyAfterLifespan() => Destroy(gameObject, lifespan);

    #region ProjectableObjectDematerialization Subscriptions
    private void ProjectableObjectDematerialization_OnStartDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        StartVFX();
    }

    private void ProjectableObjectDematerialization_OnEndDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        StopVFX();
    }

    private void ProjectableObjectDematerialization_OnObjectDematerialized(object sender, EventArgs e)
    {
        StopVFX();
        DetachVFX();
        UnsubscribeFromProjectableObjectDematerializationEvents();
        DestroyAfterLifespan();
    }
    #endregion

    #region ProjectionResetObject Subscriptions
    private void ProjectionResetObject_OnAnyStartProjectionResetObjectUse(object sender, EventArgs e)
    {
        StartVFX();
    }

    private void ProjectionResetObject_OnAnyEndProjectionResetObjectUse(object sender, EventArgs e)
    {
        StopVFX();
    }
    #endregion
}
