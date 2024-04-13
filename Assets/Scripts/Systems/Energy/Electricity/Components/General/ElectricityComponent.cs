using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ElectricityComponent : MonoBehaviour
{
    [Header("Electricity Component Settings")]
    [SerializeField] private bool on;
    [Space]
    [SerializeField] private bool transmit;
    [SerializeField] private bool source;
    [SerializeField] private bool device;
    [Space]
    public Circuit circuit;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected List<ElectricityComponent> contacts;

    [HideInInspector] public List<ElectricityComponent> allContacts;

    #region ElectricityComponentProperties
    public bool On { get { return on; } }
    public bool Transmit { get { return transmit; } }
    public bool Source { get { return source; } }
    public bool Device { get { return device; } }
    #endregion
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
