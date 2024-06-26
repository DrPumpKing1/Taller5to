using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableElectrode : Electrode
{
    [Header("Cable Specifics")]
    [SerializeField] private Transform[] corners;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float radius;
    [SerializeField] private float thickness;

    private GameObject player;

    private const string PLAYER_TAG = "PlayerTag";
    private const float DISTANCE_TO_UPDATE = 20f;

    protected override void Start()
    {
        base.Start();
        //player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
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
        //if (!CheckPlayerClose()) return;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            Vector3 direction = (corners[i + 1].position - corners[i].position).normalized;
            float distance = Vector3.Distance(corners[i].position, corners[i + 1].position);

            RaycastHit[] hits = Physics.SphereCastAll(corners[i].position, radius, direction, distance);

            List<Electrode> electrodesDetected = new List<Electrode>();

            foreach (RaycastHit hit in hits)
            {
                Electrode other = hit.collider.GetComponent<Electrode>();

                ElectrodeCollider otherCol = hit.collider.GetComponent<ElectrodeCollider>();

                if (other == null && otherCol == null) continue;

                if(other != null)
                {
                    electrodesDetected.Add(other);

                    AddContact(other);

                    continue;
                }

                electrodesDetected.Add(otherCol.Electrode);

                AddContact(otherCol.Electrode);
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
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;
        lineRenderer.positionCount = corners.Length;

        for (int i = 0; i < corners.Length; i++)
        {
            lineRenderer.SetPosition(i, corners[i].position);
        }
    }

    //public override bool CheckContact(Electrode other)
    //{
    //    if (!other.Source && !other.Sender) return false;
    //
    //    return base.CheckContact(other);
    //}

    private bool CheckPlayerClose()
    {
        if (!player) return true;
        if (Vector3.Distance(transform.position, player.transform.position) <= DISTANCE_TO_UPDATE) return true;

        return false;
    }
}
