using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    [SerializeField] private Electrode electrode;
    private List<Node> nextNodes;
    private List<Node> previousNodes;
    private Circuit circuit;
    private int weight;
    
    [SerializeField] private List<Electrode> nextElectrodes;
    [SerializeField] private List<Electrode> previousElectrodes;

    public List<Node> NextNodes {  get { return nextNodes; } }
    public List<Node> PreviousNodes { get { return previousNodes; } }
    public Electrode Component { get { return electrode; } }
    public Circuit Circuit { get { return circuit; } set { circuit = value; } }
    public int Weight { get { return weight; } set { weight = value; } }

    public Node(Electrode electrode)
    {
        this.electrode = electrode;
        this.nextNodes = new List<Node>();
        this.previousNodes = new List<Node>();
        this.circuit = new Circuit();
        this.weight = int.MaxValue;
        this.circuit.AddNode(this);
    }

    public void LabelElectrodes()
    {
        nextElectrodes = new List<Electrode>();
        previousElectrodes = new List<Electrode>();

        nextNodes.ForEach(node => nextElectrodes.Add(node.Component));
        previousNodes.ForEach(node => previousElectrodes.Add(node.Component));
    }

    public void AddContact(Node contact)
    {
        if (nextNodes.Contains(contact))
        {
            return;
        }

        nextNodes.Add(contact);
        
        LabelElectrodes();
    }

    public void RemoveContact(Node contact)
    {
        if (nextNodes.Contains(contact))
        {
            nextNodes.Remove(contact);
        }

        if (previousNodes.Contains(contact))
        {
            previousNodes.Remove(contact);
        }

        LabelElectrodes();
    }

    public void AddPreviousNode(Node node)
    {
        if (previousNodes.Contains(node)) return;

        previousNodes.Add(node);

        LabelElectrodes();
    }

    public void ResetContactsLists()
    {
        nextNodes.Clear();
        previousNodes.Clear();

        electrode.RetrieveContacts().ForEach((electrode) => nextNodes.Add(electrode.Node));
    }

    public void ResetWeight()
    {
        this.weight = int.MaxValue;
    }

    public void Broadcast()
    {
        if (electrode.Power <= 0 || electrode.Device) return;

        nextNodes.ForEach(node => {
            node.electrode.ReceiveSignal(electrode.SendSignal(node.electrode));
        });
    }

    public void BroadcastSpecific(Node node)
    {
        if (electrode.Power <= 0 || electrode.Device) return;

        if (!nextNodes.Contains(node)) return;

        node.electrode.ReceiveSignal(electrode.SendSignal(node.electrode));
    }
}
