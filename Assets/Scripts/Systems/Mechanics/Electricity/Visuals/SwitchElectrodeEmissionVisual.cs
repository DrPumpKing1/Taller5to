using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchElectrodeEmissionVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SwitchElectrode switchElectrode;
    [SerializeField] private Renderer _renderer;

    [Header("Materials")]
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    private bool IsPowered => switchElectrode.Power >= Electrode.ACTIVATION_THRESHOLD;

    private const float NOT_POWERED_TIME_THRESHOLD = 0.1f;
    private float notPoweredTimer;
    private bool previousPowered;

    private Material material;
    private bool emmissionOn;


    private void OnEnable()
    {
        switchElectrode.OnSwitchSet += SwitchElectrode_OnSwitchSet;
    }

    private void OnDisable()
    {
        switchElectrode.OnSwitchSet -= SwitchElectrode_OnSwitchSet;
    }

    private void Awake()
    {
        material = _renderer.material;

        GeneralRenderingMethods.SetMaterialEmission(material, false);
        emmissionOn = false;
    }

    private void LateUpdate()
    {
        HandlePowered();
    }

    private void HandlePowered()
    {
        if (!IsPowered)
        {
            notPoweredTimer += Time.deltaTime;

            if (notPoweredTimer >= NOT_POWERED_TIME_THRESHOLD && previousPowered)
            {
                GeneralRenderingMethods.SetMaterialEmission(material, false);
                emmissionOn = false;
                previousPowered = false;
            }
        }
        else
        {
            if (!previousPowered)
            {
                GeneralRenderingMethods.SetMaterialEmission(material, true);
                emmissionOn = true;
            }

            notPoweredTimer = 0;
            previousPowered = true;
        }

    }

    private void HandleSwitchOn()
    {
        if (switchElectrode.SwitchOn)
        {
            _renderer.material = onMaterial;
        }
        else
        {
            _renderer.material = offMaterial;
        }

        material = _renderer.material;

        if (emmissionOn)
        {
            GeneralRenderingMethods.SetMaterialEmission(material, true);
        }
        else
        {
            GeneralRenderingMethods.SetMaterialEmission(material, false);
        }
    }


    private void SwitchElectrode_OnSwitchSet(object sender, SwitchElectrode.OnSwitchSetEventArgs e)
    {
        HandleSwitchOn();
    }
}
