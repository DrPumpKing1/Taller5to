using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Circuit : IDisposable
{
    delegate void PropagateAction(Node node, int layer);

    PropagateAction resetNodes = (Node node, int layer) => node.Component?.ResetPower();

    PropagateAction routeNodes = (Node node, int layer) => {
        node.NextNodes.ForEach(nextNode =>
        {
            if(nextNode.PreviousNodes.Contains(node) && nextNode.Weight >= layer)
            {
                nextNode.Weight = layer;
                nextNode.RemoveContact(node);
                nextNode.AddContact(node);
            }

            if (!nextNode.PreviousNodes.Contains(node) && nextNode.Weight >= layer) {
                nextNode.Weight = layer;
                nextNode.RemoveContact(node);   
                nextNode.AddPreviousNode(node);
            }
        });
    };

    PropagateAction logRoute = (Node node, int layer) => {
        Debug.Log(node.Component.name + "(" + layer + "):");
        node.NextNodes.ForEach(nextNode =>
        {
            Debug.Log("-> " + nextNode.Component.name);
        });
    };

    PropagateAction powerNodes = (Node node, int layer) => node.Broadcast();

    [SerializeField] private List<Node> nodes;

    public List<Node> Elements { get { return nodes; } }
    public List<CancellationTokenSource> cancellationTokens { get; private set; }

    public Circuit()
    {
        nodes = new List<Node>();
        cancellationTokens = new List<CancellationTokenSource>();

        Electricity.Instance?.Circuits.Add(this);
    }

    public void AddNode(Node node)
    {
        if (nodes.Contains(node)) return;

        node.Circuit = this;
        nodes.Add(node);
    }

    public async void ResolveCircuit()
    {
        nodes.Sort((a, b) => Electrode.CompareElectrodes(a.Component, b.Component));
        nodes.ForEach(e => {
            e.PreviousNodes?.Clear();
            e.Component?.ResetPower();
            e.ResetContactsLists();
            e.ResetWeight();
        });

        Electricity.Instance.FlushCircuitTasks(this);
        cancellationTokens.Clear();

        if (nodes.Count <= 0) return;

        nodes.ForEach( async node =>
        {
            if (node.Component.Source)
            {
                node.Weight = 0;
                await PropagateForward(node, routeNodes);
                //await PropagateForward(node, logRoute);
            }
        });

        nodes.ForEach(async node =>
        {
            if (node.Component.Source) await PropagateForward(node, powerNodes);
        });
    }

    private async Task PropagateForward(Node startNode, PropagateAction action)
    {
        if (startNode == null) return;

        if (!nodes.Contains(startNode)) return;

        CancellationTokenSource cancelSource = new CancellationTokenSource();
        cancellationTokens.Add(cancelSource);

        await PropagateActionForward(startNode, action, cancelSource.Token, 1);

        async Task PropagateActionForward(Node node, PropagateAction action, CancellationToken cancel, int layer)
        {
            action(node, layer);

            List<Node> nodes = new List<Node>(node.NextNodes);

            foreach (Node nextNode in nodes)
            {
                if (node.PreviousNodes.Contains(nextNode))
                {
                    continue;
                }

                await Task.Delay(TimeSpan.FromSeconds(.01f));

                if(!cancel.IsCancellationRequested) await PropagateActionForward(nextNode, action, cancel, ++layer);
            }
        }
    }

    public async void UpdateNodeForward(Node node)
    {
        if (!nodes.Contains(node)) return;

        await PropagateForward(node, resetNodes);

        foreach (Node previousNode in node.PreviousNodes)
        {
            previousNode.BroadcastSpecific(node);
        }

        await PropagateForward(node, powerNodes);
    }

    public static Circuit MergeCircuit(Circuit a, Circuit b)
    {
        Circuit merge = new Circuit();

        a.nodes.ForEach(n => merge.AddNode(n));
        b.nodes.ForEach(n => merge.AddNode(n));

        a.Dispose();
        b.Dispose();

        return merge;
    }

    public void Dispose()
    {
        if (Electricity.Instance.Circuits.Contains(this))
        {
            Electricity.Instance.Circuits.Remove(this);
        }

        Electricity.Instance.FlushCircuitTasks(this);

        cancellationTokens.Clear();
        nodes.Clear();
    }
}
