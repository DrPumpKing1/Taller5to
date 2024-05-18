using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public float trapRange;
    public LayerMask trapSolid;
    public LayerMask playerLayer;

    public void ProjectRay()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, trapRange, trapSolid & playerLayer))
        {
            if(hit.collider.CompareTag("Player"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
