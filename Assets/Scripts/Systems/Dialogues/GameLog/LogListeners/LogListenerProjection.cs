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
    }

    private void OnDisable()
    {
        //SHIELDS
        ProjectionManager.OnObjectProjectionSuccess -= ProjectionManager_OnObjectProjectionSuccess;
        ProjectionManager.OnObjectProjectionFailed -= ProjectionManager_OnObjectProjectionFailed;
        ProjectionManager.OnObjectDematerialized -= ProjectionManager_OnObjectDematerialized;
        ProjectionManager.OnAllObjectsDematerialized -= ProjectionManager_OnAllObjectsDematerialized;
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
    private void ProjectionManager_OnAllObjectsDematerialized(object sender, ProjectionManager.OnAllObjectsDematerializedEventArgs e) => GameLogManager.Instance.Log($"Projection/DematerializeAllObjects/{e.projectableObjectSOs.Count}");
}
