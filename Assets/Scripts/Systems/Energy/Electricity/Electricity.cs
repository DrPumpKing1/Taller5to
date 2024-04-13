using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    [SerializeField] private List<Circuit> circuits;
    [SerializeField] private List<ElectricityComponent> components;

    public bool debug;

    public static Electricity Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateComponents();
        DetectCircuits();
        circuits.ForEach(circuit => circuit.ResolveCircuit());
    }

    private void UpdateComponents()
    {
        List<ElectricityComponent> disorderedComponents = Resources.FindObjectsOfTypeAll<ElectricityComponent>().ToList();

        IEnumerable<ElectricityComponent> orderedComponents = disorderedComponents.OrderBy((component) => {
            if (component.source) return 0;
            else if (component.transmit) return 1;
            else if (component.device) return 2;
            else return 3;
        });

        components = orderedComponents.ToList();

        BillaterallizeContacts();
    }

    private void DetectCircuits()
    {
        circuits.ForEach(c => c.Dispose());
        circuits.Clear();

        foreach (ElectricityComponent component in components)
        {
            EvaluateComponent(component);
        }
    }

    private void BillaterallizeContacts()
    {
        components.ForEach((c) => c.allContacts = new List<ElectricityComponent>(c.GetContacts()));

        foreach (ElectricityComponent component in components)
        {
            List<ElectricityComponent> contacts = component.GetContacts();
            contacts.ForEach((c) =>
            {
                if (!c.allContacts.Contains(component) && c.CheckValidElectricalContact(component)) c.allContacts.Add(component);
            });
        }
    }

    private void EvaluateComponent(ElectricityComponent component)
    {
        if (component.circuit.GetComponents().Count == 0) component.circuit = new Circuit(new List<ElectricityComponent>() { component });

        foreach (ElectricityComponent contact in component.allContacts)
        {
            if(contact.circuit != null)
            {
                Circuit mergeCircuit = component.circuit.ConnectCircuits(contact.circuit);
                continue;
            }

            component.circuit.ConnectComponent(contact);
        }
    }

    public void AddCircuit(Circuit circuit)
    {
        if(circuits.Contains(circuit)) return;
        
        circuits.Add(circuit);
    }

    public void RemoveCircuit(Circuit circuit)
    {
        if (!circuits.Contains(circuit)) return;
        
        circuits.Remove(circuit);
    }
}
