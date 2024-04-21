using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MagicBoxDevice : MonoBehaviour
{
    [Header("Electrical Component")]
    [SerializeField] private Electrode electrode;

    [Header("Device Settings")]
    [SerializeField] private Transform pistonHead;
    [SerializeField] private float extension;
    Vector3 initialPosition;
    Vector3 extendedPosition;

    [Header("Piston Head Movement")]
    [SerializeField] private float pistonHeadSpeed;
    [SerializeField] private AnimationCurve movementCurve;

    [Header("Device Control")]
    [SerializeField] private bool state;
    private bool power => electrode.Power >= Electrode.ACTIVATION_THRESHOLD;
    private bool coherence => state == power;

    private void Start()
    {
        initialPosition = pistonHead.localPosition;
        extendedPosition = pistonHead.localPosition + Vector3.up * extension;
    }

    private void Update()
    {
        if (coherence) return;

        state = power;
        TriggerMovement();
    }

    private void TriggerMovement()
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
