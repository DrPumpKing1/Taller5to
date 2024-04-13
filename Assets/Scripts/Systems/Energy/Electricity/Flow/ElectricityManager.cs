using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    [Header("Electricity Settings")]
    [SerializeField] private List<Circuit> circuits;
    [SerializeField] private List<ElectricityComponent> components;

    [Header("Debug")]
    public bool debug;

    public static ElectricityManager Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        UpdateComponents();
        DetectCircuits();
        circuits.ForEach(circuit => circuit.ResolveCircuit());
    }
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ElectricityManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void UpdateComponents()
    {
        List<ElectricityComponent> disorderedComponents = Resources.FindObjectsOfTypeAll<ElectricityComponent>().ToList();

        IEnumerable<ElectricityComponent> orderedComponents = disorderedComponents.OrderBy((component) => {
            if (component.Source) return 0;
            else if (component.Transmit) return 1;
            else if (component.Device) return 2;
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
