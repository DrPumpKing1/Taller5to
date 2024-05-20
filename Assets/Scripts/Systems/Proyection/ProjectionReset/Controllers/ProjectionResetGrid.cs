using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionResetGrid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ProjectionManager.Instance.DematerializeAllObjects();
    }
}
