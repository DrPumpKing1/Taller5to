using JetBrains.Annotations;
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
    delegate void PropagateAction(Node node, int layer, float elementCount, List<Node> nodesEvaluated);

    PropagateAction resetNodes = (Node node, int layer, float elementCount, List<Node> nodesEvaluated) => node.Component?.ResetPower();

    PropagateAction routeNodes = (Node node, int layer, float elementCount, List<Node> nodesEvaluated) => {
        node.ContactNodes.ForEach(contact => {
            if (!nodesEvaluated.Contains(contact) && !contact.Component.Source) {
                float value = (node.Weight - (1 * (elementCount))) / (2 * (node.ContactNodes.Count) * (node.Component.Source ? (elementCount + 1) / 2 : 1));
                if (node.Component.DebugTool || contact.Component.DebugTool) Debug.Log($"{node.Component.name} add {value} weight to {contact.Component.name}");
                contact.Weight += value;
            };
        });
    };

    PropagateAction powerNodes = (Node node, int layer, float elementCount, List<Node> nodesEvaluated) => node.ContactNodes.ForEach(contact => {
        contact.BroadcastSpecific(node);
    });

    [SerializeField] private List<Node> nodes;

    public List<CancellationTokenSource> cancellationTokens { get; private set; }

    public List<Node> Elements { get { return nodes; } }

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
            e.Component?.ResetPower();
            e.ResetWeight();
        });

        Electricity.Instance.FlushCircuitTasks(this);
        cancellationTokens.Clear();

        if (nodes.Count <= 0) return;

        List<Node> nodesCopy = new List<Node>(nodes);

        List<Node> evaluatedNode = new List<Node>();

        foreach (Node node in nodesCopy)
        {
            if(node.Component.Source)
            {
                evaluatedNode.Clear();
                await PropagateForward(node, routeNodes, false, true, evaluatedNode);
            }
        }

        foreach (Node node in nodesCopy)
        {
            if (node.Component.Source)
            {
                await PropagateForward(node, powerNodes, false, false, evaluatedNode);
            }
        }
    }

    private async Task PropagateForward(Node startNode, PropagateAction action, bool debug, bool useEvaluation, List<Node> evaluatedNode) 
    { 
        if (startNode == null) return;

        if (!nodes.Contains(startNode)) return;

        CancellationTokenSource cancelSource = new CancellationTokenSource();
        cancellationTokens.Add(cancelSource);

        await PropagateActionForward(startNode, action, 1, cancelSource.Token, debug);

        async Task PropagateActionForward(Node node, PropagateAction action, int layer, CancellationToken cancel, bool debug)
        {
            if(debug)Debug.Log("CALLING " + node.Component.name);
            action(node, layer, this.nodes.Count, evaluatedNode);
            evaluatedNode.Add(node);

            List<Node> nodes = new List<Node>(node.ContactNodes);

            foreach (Node nextNode in nodes)
            {
                if (node.Weight <= nextNode.Weight)
                {
                    continue;
                }

                if(evaluatedNode.Contains(nextNode) && useEvaluation)
                {
                    continue;
                }

                await Task.Delay(TimeSpan.FromSeconds(.01f));

                await PropagateActionForward(nextNode, action, ++layer, cancel, debug);
            }
        }
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

    public async void UpdateNodeForward(Node node)
    {
        if (!nodes.Contains(node)) return;

        await PropagateForward(node, resetNodes, false, false, new List<Node>());

        foreach (Node previousNode in node.ContactNodes)
        {
            previousNode.BroadcastSpecific(node);
        }

        await PropagateForward(node, powerNodes, false, false, new List<Node>());
    }
}
