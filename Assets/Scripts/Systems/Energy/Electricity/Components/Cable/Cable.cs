using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : ElectricityComponent
{
    [Header("Components")]
    [SerializeField] private Transform[] cableCorners;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Cable Settings")]
    [SerializeField] private float cornerRadius;

    protected override void Start()
    {
        LabelLineRenderer();
    }

    protected override void Update()
    {
        base.Update();
        LabelLineRenderer();
    }

    private void FixedUpdate()
    {
        FindNearElectricalComponents();
    }

    private void FindNearElectricalComponents()
    {
        contacts = new List<ElectricityComponent>();

        for (int i = 0; i < cableCorners.Length - 1; i++)
        {
            Vector3 direction = (cableCorners[i + 1].position - cableCorners[i].position).normalized;
            float distance = Vector3.Distance(cableCorners[i].position, cableCorners[i + 1].position);

            RaycastHit[] hits = Physics.SphereCastAll(cableCorners[i].position, cornerRadius, direction, distance);

            foreach (RaycastHit hit in hits )
            {
                if( hit.collider == null ) continue;

                if (hit.collider.gameObject == gameObject) continue;

                ElectricityComponent otherComponent = hit.collider.GetComponent<ElectricityComponent>();

                if (otherComponent == null) continue;
                
                if (contacts.Contains(otherComponent)) continue;

                if(!CheckValidElectricalContact(otherComponent)) continue;

                contacts.Add(otherComponent);
            }
        }
    }

    private void LabelLineRenderer()
    {
        lineRenderer.startWidth = cornerRadius;
        lineRenderer.endWidth = cornerRadius;
        lineRenderer.positionCount = cableCorners.Length;

        for (int i = 0; i < cableCorners.Length; i++)
        {
            lineRenderer.SetPosition(i, cableCorners[i].position);
        }
    }

    public override bool CheckValidElectricalContact(ElectricityComponent other)
    {
        if (!other.Source && !other.Transmit) return false;

        return base.CheckValidElectricalContact(other);
    }
}
