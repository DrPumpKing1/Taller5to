using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectionPlatformProjectionVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectionPlatformProjection projectionPlatformProjection;
    [SerializeField] private VisualEffect projectionPlatformProjectionVFX;
    [Space]
    [SerializeField] private List<ProjectionVFXSetting> projectionVFXSettings;

    private const string BOX_CENTER_PROPERTY = "BoxCenter";
    private const string BOX_SIZE_PROPERTY = "BoxSize";

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
        projectionPlatformProjection.OnStartProjection += ProjectionPlatformProjection_OnStartProjection;
        projectionPlatformProjection.OnEndProjection += ProjectionPlatformProjection_OnEndProjection;
    }

    private void OnDisable()
    {
        projectionPlatformProjection.OnStartProjection -= ProjectionPlatformProjection_OnStartProjection;
        projectionPlatformProjection.OnEndProjection -= ProjectionPlatformProjection_OnEndProjection;
    }

    private void Start()
    {
        InitializeVFX();
    }

    private void InitializeVFX()
    {
        projectionPlatformProjectionVFX.gameObject.SetActive(true);
        projectionPlatformProjectionVFX.Stop();
    }

    private void SetVFXBoxCenter(Vector3 center)
    {
        if (projectionPlatformProjectionVFX.HasVector3(BOX_CENTER_PROPERTY))
        {
            projectionPlatformProjectionVFX.SetVector3(BOX_CENTER_PROPERTY, center);
        }
    }

    private void SetVFXBoxSize(Vector3 size)
    {
        if (projectionPlatformProjectionVFX.HasVector3(BOX_SIZE_PROPERTY))
        {
            projectionPlatformProjectionVFX.SetVector3(BOX_SIZE_PROPERTY, size);
        }
    }

    private ProjectionVFXSetting GetVFXSettingsByProjectableObjectSO(ProjectableObjectSO projectableObjectSO)
    {
        foreach(ProjectionVFXSetting projectionVFXSetting in projectionVFXSettings)
        {
            if (projectableObjectSO == projectionVFXSetting.projectableObjectSO) return projectionVFXSetting;
        }

        return null;
    }

    private void StartVFX(ProjectableObjectSO projectableObjectSO, Vector3 projectionPlatformPosition, Vector2 orientation)
    {
        ProjectionVFXSetting projectionVFXSetting = GetVFXSettingsByProjectableObjectSO(projectableObjectSO);

        if(projectionVFXSetting == null)
        {
            Debug.Log("ProjectionVFX Setting not found for ProjectableObjectSelected, VFX will be ignored");
            return;
        }

        Vector3 boxCenter = projectionPlatformPosition + projectionVFXSetting.boxOffset;
        Vector3 boxSize = projectionVFXSetting.boxSize;

        if(projectionVFXSetting.rectifyDueToOrientation) boxSize = RectifyBoxSizeDueToOrientation(boxSize, orientation);

        SetVFXBoxCenter(boxCenter);
        SetVFXBoxSize(boxSize);

        projectionPlatformProjectionVFX.Play();
    }

    private Vector3 RectifyBoxSizeDueToOrientation(Vector3 boxSize, Vector2 orientation)
    {
        if(orientation == Vector2.zero) return boxSize;
        if (orientation.x == orientation.y) return boxSize;

        if (Math.Abs(orientation.x) > Math.Abs(orientation.y)) return boxSize;

        if(Math.Abs(orientation.x) < Math.Abs(orientation.y))
        {
            Vector3 rectifiedBoxSize = new Vector3(boxSize.z, boxSize.y, boxSize.x);
            return rectifiedBoxSize;
        }

        return boxSize;
    }

    private void ProjectionPlatformProjection_OnStartProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        StartVFX(e.projectableObjectSO, e.projectionPlatformProjection.transform.position, e.orientation);
    }

    private void ProjectionPlatformProjection_OnEndProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        projectionPlatformProjectionVFX.Stop();
    }
}
