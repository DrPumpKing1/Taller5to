using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerProjection : MonoBehaviour
{
    private void OnEnable()
    {
        //PROJECTION
        ProjectionManager.OnObjectProjectionSuccess += ProjectionManager_OnObjectProjectionSuccess;
        ProjectionManager.OnObjectProjectionFailed += ProjectionManager_OnObjectProjectionFailed;

        ProjectionManager.OnObjectDematerialized += ProjectionManager_OnObjectDematerialized;
        ProjectionManager.OnAllObjectsDematerialized += ProjectionManager_OnAllObjectsDematerialized;

        ProjectionManager.OnObjectForceDematerialized += ProjectionManager_OnObjectForceDematerialized;
        ProjectionManager.OnAllObjectsForceDematerialized += ProjectionManager_OnAllObjectsForceDematerialized;
    }

    private void OnDisable()
    {
        //SHIELDS
        ProjectionManager.OnObjectProjectionSuccess -= ProjectionManager_OnObjectProjectionSuccess;
        ProjectionManager.OnObjectProjectionFailed -= ProjectionManager_OnObjectProjectionFailed;

        ProjectionManager.OnObjectDematerialized -= ProjectionManager_OnObjectDematerialized;
        ProjectionManager.OnAllObjectsDematerialized -= ProjectionManager_OnAllObjectsDematerialized;

        ProjectionManager.OnObjectForceDematerialized -= ProjectionManager_OnObjectForceDematerialized;
        ProjectionManager.OnAllObjectsForceDematerialized -= ProjectionManager_OnAllObjectsForceDematerialized;
    }

    private void ProjectionManager_OnObjectProjectionSuccess(object sender, ProjectionManager.OnProjectionEventArgs e)
    {
        GameLogManager.Instance.Log($"Projection/MaterializeObject/{e.projectableObjectSO.id}");
        GameLogManager.Instance.Log($"Projection/MaterializeObjectExact/{e.projectableObjectSO.id}/{e.projectionPlatformID}");
    }
    private void ProjectionManager_OnObjectProjectionFailed(object sender, ProjectionManager.OnProjectionEventArgs e)
    {
        GameLogManager.Instance.Log($"Projection/FailMaterializeObject/{e.projectableObjectSO.id}");
        GameLogManager.Instance.Log($"Projection/FailMaterializeObjectExact/{e.projectableObjectSO.id}/{e.projectionPlatformID}");
    }

    private void ProjectionManager_OnObjectDematerialized(object sender, ProjectionManager.OnProjectionEventArgs e)
    {
        GameLogManager.Instance.Log($"Projection/DematerializeObject/{e.projectableObjectSO.id}");
        GameLogManager.Instance.Log($"Projection/DematerializeObjectExact/{e.projectableObjectSO.id}/{e.projectionPlatformID}");
    }
    private void ProjectionManager_OnAllObjectsDematerialized(object sender, ProjectionManager.OnAllObjectsDematerializedEventArgs e)
    {
        GameLogManager.Instance.Log($"Projection/DematerializeAllObjects");
        GameLogManager.Instance.Log($"Projection/DematerializeAllObjectsCount/{e.projectableObjectSOs.Count}");
    }
    private void ProjectionManager_OnObjectForceDematerialized(object sender, ProjectionManager.OnProjectionEventArgs e)
    {
        GameLogManager.Instance.Log($"Projection/ForceDematerializeObject/{e.projectableObjectSO.id}");
        GameLogManager.Instance.Log($"Projection/ForceDematerializeObjectExact/{e.projectableObjectSO.id}/{e.projectionPlatformID}");
    }

    private void ProjectionManager_OnAllObjectsForceDematerialized(object sender, ProjectionManager.OnAllObjectsDematerializedEventArgs e)
    {
        GameLogManager.Instance.Log($"Projection/ForceDematerializeAllObjects");
        GameLogManager.Instance.Log($"Projection/ForceDematerializeAllObjectsCount/{e.projectableObjectSOs.Count}");
    }
}
