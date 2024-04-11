using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualInteractionInput : InteractionInput, IActionHandler
{
    [Header("Interaction")]
    private ActionListener interactionDownListener;
    private ActionListener interactionUpListener;
    private ActionListener interactionHoldListener;

    [Header("Interaction Alternate")]
    private ActionListener interactionAltDownListener;
    private ActionListener interactionAltUpListener;
    private ActionListener interactionAltHoldListener;

    protected override void Awake()
    {
        base.Awake();
        SetUpActionListener();
    }

    public override bool CanProcessInteractionInput() => true;

    public override bool GetInteractionDown()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = interactionDownListener.value.b;
        return interactionInput;
    }

    public override bool GetInteractionUp()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = interactionUpListener.value.b;
        return interactionInput;
    }

    public override bool GetInteractionHold()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionInput = interactionHoldListener.value.b;
        return interactionInput;
    }

    public override bool GetInteractionAlternateDown()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = interactionAltDownListener.value.b;
        return interactionAlternateInput;
    }

    public override bool GetInteractionAlternateUp()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = interactionAltUpListener.value.b;
        return interactionAlternateInput;
    }

    public override bool GetInteractionAlternateHold()
    {
        if (!CanProcessInteractionInput()) return false;

        bool interactionAlternateInput = interactionAltHoldListener.value.b;
        return interactionAlternateInput;
    }

    public void SetUpActionListener()
    {
        interactionDownListener = gameObject.AddComponent<ActionListener>();
        interactionDownListener.SetActionName("Interaction Down");

        interactionUpListener = gameObject.AddComponent<ActionListener>();
        interactionUpListener.SetActionName("Interaction Up");

        interactionHoldListener = gameObject.AddComponent<ActionListener>();
        interactionHoldListener.SetActionName("Interaction");

        interactionAltDownListener = gameObject.AddComponent<ActionListener>();
        interactionAltDownListener.SetActionName("Interaction Alt Down");

        interactionAltUpListener = gameObject.AddComponent<ActionListener>();
        interactionAltUpListener.SetActionName("Interaction Alt Up");

        interactionAltHoldListener = gameObject.AddComponent<ActionListener>();
        interactionAltHoldListener.SetActionName("Interaction Alt");
    }
}
