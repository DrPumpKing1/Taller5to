using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableElectrode : Electrode
{
    [Header("Cable Specifics")]
    [SerializeField] private Transform[] corners;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float radius;

    private void Update()
    {
        LabelLineRenderer();
    }

    private void FixedUpdate()
    {
        FindNearElectricalComponents();
    }

    private void FindNearElectricalComponents()
    {
        for (int i = 0; i < corners.Length - 1; i++)
        {
            Vector3 direction = (corners[i + 1].position - corners[i].position).normalized;
            float distance = Vector3.Distance(corners[i].position, corners[i + 1].position);

            RaycastHit[] hits = Physics.SphereCastAll(corners[i].position, radius, direction, distance);

            List<Electrode> electrodesDetected = new List<Electrode>();

            foreach (RaycastHit hit in hits)
            {
                Electrode other = hit.collider.GetComponent<Electrode>();

                if (other == null) continue;

                electrodesDetected.Add(other);

                AddContact(other);
            }

            List<Electrode> ToRemove = new List<Electrode>();

            foreach (Electrode contact in contacts)
            {
                if (!electrodesDetected.Contains(contact)) ToRemove.Add(contact);
            }

            ToRemove.ForEach(contact => RemoveContact(contact));
        }
    }

    private void LabelLineRenderer()
    {
        lineRenderer.startWidth = radius;
        lineRenderer.endWidth = radius;
        lineRenderer.positionCount = corners.Length;

        for (int i = 0; i < corners.Length; i++)
        {
            lineRenderer.SetPosition(i, corners[i].position);
        }
    }

    public override bool CheckContact(Electrode other)
    {
        if (!other.Source && !other.Sender) return false;

        return base.CheckContact(other);
    }
}
