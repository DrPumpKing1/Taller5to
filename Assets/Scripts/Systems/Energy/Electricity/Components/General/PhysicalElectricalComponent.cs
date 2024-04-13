using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalElectricalComponent : ElectricityComponent
{
    protected Collider box;

    protected override void Start()
    {
        base.Start();
        box = GetComponent<Collider>();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        Collider otherCollider = collision.collider;

        if (debug) Debug.Log(this.name + " -> " + otherCollider.name);

        if (otherCollider == null) return;

        if(otherCollider.gameObject == gameObject) return;

        ElectricityComponent otherComponent = otherCollider.gameObject.GetComponent<ElectricityComponent>();

        if (otherComponent == null) return;

        if (contacts.Contains(otherComponent)) return;

        contacts.Add(otherComponent);
    }

    protected void OnCollisionStay(Collision collision)
    {
        Collider otherCollider = collision.collider;

        if (otherCollider == null) return;

        if (otherCollider.gameObject == gameObject) return;

        ElectricityComponent otherComponent = otherCollider.gameObject.GetComponent<ElectricityComponent>();

        if (otherComponent == null) return;

        if (contacts.Contains(otherComponent)) return;

        contacts.Add(otherComponent);
    }

    protected void OnCollisionExit(Collision collision)
    {
        Collider otherCollider = collision.collider;

        if (debug) Debug.Log(this.name + " X " + otherCollider.name);

        if (otherCollider == null) return;

        if (otherCollider.gameObject == gameObject) return;

        ElectricityComponent otherComponent = otherCollider.gameObject.GetComponent<ElectricityComponent>();

        if (otherComponent == null) return;

        if (!contacts.Contains(otherComponent)) return;

        contacts.Remove(otherComponent);
    }
}
