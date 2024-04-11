using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

[System.Serializable]
public class Circuit : IDisposable
{
    [SerializeField] private List<ElectricityComponent> components;

    public Circuit(List<ElectricityComponent> components) 
    {
        this.components = components;

        Electricity.Instance.AddCircuit(this);
    }

    public void ResolveCircuit()
    {
        components.ForEach((c) => c.Switch(c.source & c.On));

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

        if(Electricity.Instance.debug) Debug.Log(sender.name + " -> " + component.name);

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

        Electricity.Instance.RemoveCircuit(this);
        Electricity.Instance.RemoveCircuit(circuit);

        this.Dispose();
        circuit.Dispose();

        Electricity.Instance.AddCircuit(connectedCircuit);

        return connectedCircuit;
    }

    public List<ElectricityComponent> GetComponents()
    {
        return components;
    }

    public List<ElectricityComponent> GetSources()
    {
        return components.FindAll((c) => c.source);
    }

    public List<ElectricityComponent> GetTransmitters()
    {
        return components.FindAll((c) => c.transmit);
    }

    public List<ElectricityComponent> GetDevices()
    {
        return components.FindAll((c) => c.device);
    }

    public void Dispose()
    {
        components.Clear();
    }
}
