using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ElectricityComponent : MonoBehaviour
{
    [SerializeField] protected bool on;

    public bool On { get { return on; } }

    public bool transmit;

    public bool source;

    public bool device;

    public bool debug;

    public Circuit circuit;

    protected List<ElectricityComponent> contacts;

    [HideInInspector] public List<ElectricityComponent> allContacts;

    protected virtual void Start()
    {
        contacts = new List<ElectricityComponent>();
    }

    protected virtual void Update()
    {
        UpdateElectricalContacts();
    }

    public virtual void ElectricalState()
    {
        foreach (ElectricityComponent contact in allContacts)
        {
            if(contact.source) on |= contact.on;

            if(contact.device) contact.on |= on;

            if (contact.transmit)
            {
                on |= contact.on;

                if (transmit || source)
                {
                    contact.on |= on;
                }
            }

            if(debug) Debug.Log(name + " (" + (on ? "1" : "0") + ") " + contact.name + "(" + (contact.on ? "1" : "0") + ")");
        }
    }

    protected virtual void UpdateElectricalContacts()
    {
        List<ElectricityComponent> tagToRemove = new List<ElectricityComponent>();

        foreach (ElectricityComponent contact in contacts)
        {
            if(debug) Debug.Log(this.name + " contacts " + contact.name);

            if (CheckValidElectricalContact(contact)) continue;

            tagToRemove.Add(contact);
        }

        tagToRemove.ForEach(c => contacts.Remove(c));
    }

    public virtual bool CheckValidElectricalContact(ElectricityComponent other)
    {
        if(!this.transmit && !other.transmit) return false;

        return true;
    }

    public void Switch(bool on)
    {
        if(this.on == on) return;

        this.on = on;
    }

    public List<ElectricityComponent> GetContacts()
    {
        return contacts;
    }
}
