using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MagicBoxDevice : ActivableDevice
{
    [Header("Device Settings")]
    [SerializeField] private Transform pistonHead;
    [SerializeField] private float extension;

    [Header("Piston Head Movement")]
    [SerializeField] private float pistonHeadSpeed;
    [SerializeField] private AnimationCurve movementCurve;

    private Vector3 initialPosition;
    private Vector3 extendedPosition;

    protected override void Start()
    {
        base.Start();
        initialPosition = pistonHead.localPosition;
        extendedPosition = pistonHead.localPosition + Vector3.up * extension;
    }

    protected override void ToggleActivation()
    {
        StopAllCoroutines();
        StartCoroutine(MovePistonHeadToPosition(state ? extendedPosition : initialPosition));
    }

    private IEnumerator MovePistonHeadToPosition(Vector3 position)
    {
        Vector3 startPosition = pistonHead.localPosition;
        Vector3 endPosition = position;
        float distance = Vector3.Distance(startPosition, endPosition);
        float time = distance / pistonHeadSpeed;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;

            pistonHead.localPosition = Vector3.Lerp(startPosition, endPosition, movementCurve.Evaluate(t / time));

            yield return null;
        }

        pistonHead.localPosition = endPosition;
    }
}
