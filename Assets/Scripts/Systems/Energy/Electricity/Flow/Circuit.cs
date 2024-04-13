using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Circuit : IDisposable
{
    [SerializeField] private List<ElectricityComponent> components;

    public Circuit(List<ElectricityComponent> components) 
    {
        this.components = components;

        ElectricityManager.Instance.AddCircuit(this);
    }

    public void ResolveCircuit()
    {
        components.ForEach((c) => c.Switch(c.Source & c.On));

        List<ElectricityComponent> sources = new List<ElectricityComponent>(GetSources());

        sources.ForEach(s =>
        {
            s.ElectricalState();
            s.allContacts.ForEach(c => {
                RecursiveStateHandler(s, c, new List<ElectricityComponent>() { s });
            });
        }); 
    }

    private void RecursiveStateHandler(ElectricityComponent sender, ElectricityComponent component, List<ElectricityComponent> evaluation)
    {
        if (evaluation.Contains(component)) return;

        if(ElectricityManager.Instance.debug) Debug.Log(sender.name + " -> " + component.name);

        component.ElectricalState();

        evaluation.Add(component);

        component.allContacts.ForEach(c => {
            RecursiveStateHandler(component, c, evaluation);
            
        });
    }

    public void ConnectComponent(ElectricityComponent component)
    {
        if (components.Contains(component)) return;

        component.circuit = this;

        components.Add(component);
    }

    public void DisconnectComponent(ElectricityComponent component)
    {
        if(!components.Contains(component)) return;
        
        component.circuit = null;

        components.Remove(component);
    }

    public Circuit ConnectCircuits(Circuit circuit)
    {
        if(this == circuit) return this;

        List<ElectricityComponent> otherComponents = circuit.GetComponents();

        List<ElectricityComponent> connectedComponents = new List<ElectricityComponent>(components);

        connectedComponents.AddRange(otherComponents);

        Circuit connectedCircuit = new Circuit(connectedComponents);

        foreach (ElectricityComponent component in connectedComponents)
        {
            component.circuit = connectedCircuit;
        }

        ElectricityManager.Instance.RemoveCircuit(this);
        ElectricityManager.Instance.RemoveCircuit(circuit);

        this.Dispose();
        circuit.Dispose();

        ElectricityManager.Instance.AddCircuit(connectedCircuit);

        return connectedCircuit;
    }

    public List<ElectricityComponent> GetComponents()
    {
        return components;
    }

    public List<ElectricityComponent> GetSources()
    {
        return components.FindAll((c) => c.Source);
    }

    public List<ElectricityComponent> GetTransmitters()
    {
        return components.FindAll((c) => c.Transmit);
    }

    public List<ElectricityComponent> GetDevices()
    {
        return components.FindAll((c) => c.Device);
    }

    public void Dispose()
    {
        components.Clear();
    }
}
