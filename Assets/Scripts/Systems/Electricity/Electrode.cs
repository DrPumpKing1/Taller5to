using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Electrode : MonoBehaviour
{
    public delegate void ReceiveSignalDel();
    public event ReceiveSignalDel OnReceiveSignal; 

    protected const float MAX_POWER = 10000;
    protected const float DECAY_MULTIPLIER = 50f;
    public const float ACTIVATION_THRESHOLD = 20f;
    public const float ELECTRICITY_TICK_DURATION = 1f;

    [Header("Debug Tools")]
    [SerializeField] protected bool debug;
    public bool DebugTool { get { return debug; } }

    [Header("Electricity Component")]

    [SerializeField] protected float sourcePower;
    [SerializeField] protected float power;
    public float Power { get { return power; } }
    public float SourcePower { get { return sourcePower; } }

    [Header("Transmission Variables")]
    [SerializeField] protected Signal signal;
    [SerializeField] protected AnimationCurve powerCurve;
    [SerializeField] protected float resistance;
    [SerializeField] protected float powerTimer;

    public enum ComponentType
    {
        source,
        sender,
        device
    }

    [SerializeField] private ComponentType[] type;

    private bool source;
    public bool Source { get { return source; } }
    private bool sender;
    public bool Sender { get { return sender; } }
    private bool device;
    public bool Device { get { return device; } }

    [Header("Circuit Representation")]
    [SerializeField] protected Node node;
    public Node? Node { get { return node; } set { node = value; } }

    [SerializeField] protected List<Electrode> contacts;

    protected virtual void Start()
    {
        contacts = new List<Electrode>();
        SetElectrodeType();
        ResetPower();
        node = new Node(this);
    }

    protected virtual void Update()
    {
        if (signal != null) power = signal.GetPower(powerTimer);

        if (powerTimer < ELECTRICITY_TICK_DURATION) powerTimer += Time.deltaTime;
    }

    protected void SetElectrodeType()
    {
        List<ComponentType> types = type.ToList();

        if (type.Contains(ComponentType.source) && !type.Contains(ComponentType.device)) source = true;

        if (type.Contains(ComponentType.sender)) sender = true;

        if (type.Contains(ComponentType.device) && !type.Contains(ComponentType.source)) device = true;
    }

    public void AddContact(Electrode other, bool enableContact)
    {
        if (!CheckContact(other)) return;

        if (contacts.Contains(other)) return;

        contacts.Add(other);

        node.AddContact(other.node);

        if (enableContact)
        {
            other.AddContact(this, false);
            return;
        }
        
        if (!Node.Circuit.Elements.Contains(other.Node)) Electricity.Instance.ConnectComponents(this, other);
    }

    public void AddContact(Electrode other)
    {
        AddContact(other, true);
    }

    public void RemoveContact(Electrode other, bool enableContact)
    {
        if (!contacts.Contains(other)) return;

        contacts.Remove(other);

        node.RemoveContact(other.node);

        if (enableContact)
        {
            other.RemoveContact(this, false);
            return;
        }

        Electricity.Instance.DisconnectComponents(Node.Circuit);
    }

    public void RemoveContact(Electrode other)
    {
        RemoveContact(other, true);
    }

    public virtual bool CheckContact(Electrode other)
    {
        if (!Electricity.Instance.Electrodes.Contains(other)) return false;

        if (!this.sender && !other.sender) return false;

        return true;
    }

    public virtual void ReceiveSignal(float intensity)
    {
        if (intensity <= 0) return;

        signal = new Signal(intensity, powerCurve);

        powerTimer = 0f;

        power = signal.GetPower(powerTimer);

        if (OnReceiveSignal != null) OnReceiveSignal();
    }

    public virtual float SendSignal(Electrode otherComponent)
    {
        float distance = Vector3.Distance(transform.position, otherComponent.transform.position);

        float intensity = Mathf.Max(this.power - (DECAY_MULTIPLIER * distance + resistance), 0f);

        if (debug) Debug.Log($"{name} sending signal of {intensity} to {otherComponent.name}");

        return intensity;
    }

    public void SetPower(float power)
    {
        if (!source) return;

        this.power = power;

        Electricity.Instance.UpdateElectrode(this);
    }

    public virtual void ResetPower()
    {
        if (!source) power = 0;
    }

    public List<Electrode> RetrieveContacts()
    {
        return contacts;
    }

    public static int CompareElectrodes(Electrode a, Electrode b)
    {
        return a.GetElectrodePriority() - b.GetElectrodePriority();
    }

    private int GetElectrodePriority()
    {
        if (source) return 0;
        else if (sender) return 5;
        else return 10;
    }

    private void OnDestroy()
    {
        Electricity.Instance.RemoveComponentFromList(this);
    }
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position + new Vector3(0, 1, 0), $"Weight: {node.Weight}");
        Handles.Label(transform.position + new Vector3(0, 1.2f, 0), $"Voltage: {power}");
    }
}
