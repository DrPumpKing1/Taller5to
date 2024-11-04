using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    [Header("Compoments")]
    [SerializeField] private Transform collidersTransform;

    [Header("Settings")]
    [SerializeField] private List<Electrode> controllingNodes; //All should be deenergized to deactivate shield
    [SerializeField] private List<ElectricityCutNode> electricityCutNodes;

    [Header("Booleans")]
    [SerializeField] private bool shieldActive;

    public bool ShieldActive => shieldActive;

    private const float NOT_POWERED_TIME_THRESHOLD = 0.45f;
    private float notPoweredTimer;
    private bool previousPowered;

    public static event EventHandler OnAnyBossShieldActivated;
    public static event EventHandler OnAnyBossShieldDeactivated;

    public event EventHandler OnBossShieldActivated;
    public event EventHandler OnBossShieldDeactivated;

    private void Start()
    {
        SetPreviouslypowered(true);
        ResetTimer();
    }

    private void Update()
    {
        HandleShieldActive();
    }


    private void HandleShieldActive()
    {
        if (!NodesEnergyzed())
        {
            notPoweredTimer += Time.deltaTime;

            if (notPoweredTimer >= NOT_POWERED_TIME_THRESHOLD && previousPowered)
            {
                SetShieldActive(false);
                SetPreviouslypowered(false);
            }
        }
        else
        {
            if (!previousPowered)
            {
                SetShieldActive(true);
            }

            ResetTimer();
            SetPreviouslypowered(true);
        }
    }

    private bool NodesEnergyzed()
    {
        foreach (Electrode controllingNode in controllingNodes)
        {
            if (controllingNode.Power >= Electrode.ACTIVATION_THRESHOLD) return true;
        }

        return false;
    }
    private void SetShieldActive(bool active)
    {
        SetColliders(active);
        
        if (active) CutNodesElectricity();
        else LetNodesElectricity();

        shieldActive = active;

        if (active)
        {
            OnAnyBossShieldActivated?.Invoke(this, EventArgs.Empty);
            OnBossShieldActivated?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnAnyBossShieldDeactivated?.Invoke(this, EventArgs.Empty);
            OnBossShieldDeactivated?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetColliders(bool active) => collidersTransform.gameObject.SetActive(active);

    private void CutNodesElectricity()
    {
        foreach(ElectricityCutNode electricityCutNode in electricityCutNodes)
        {
            electricityCutNode.CutElectricity();
        }
    }

    private void LetNodesElectricity()
    {
        foreach (ElectricityCutNode electricityCutNode in electricityCutNodes)
        {
            electricityCutNode.LetElectricity();
        }
    }

    private void ResetTimer() => notPoweredTimer = 0f;
    private void SetPreviouslypowered(bool powered) => previousPowered = powered;


}
