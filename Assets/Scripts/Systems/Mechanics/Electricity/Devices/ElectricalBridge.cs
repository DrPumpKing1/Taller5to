using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalBridge : MonoBehaviour
{
    [Header("Electrical Component")]
    [SerializeField] private Electrode electrode;

    [Header("Device Settings")]
    [SerializeField] private Transform model;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float extension;
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float scalingTime;
    [SerializeField] private AnimationCurve scalingCurve;

    [Header("Device Control")]
    [SerializeField] private bool state;
    private bool power => electrode.Power >= Electrode.ACTIVATION_THRESHOLD;
    private bool coherence => state == power;

    private Vector3 minPos;
    private Vector3 maxPos;

    private void Start()
    {
        minScale = model.localScale;
        maxScale = new Vector3(model.localScale.x, model.localScale.y, model.localScale.z + extension);

        minPos = model.localPosition;
        maxPos = new Vector3(model.localPosition.x, model.localPosition.y, model.localPosition.z + extension/2);
    }

    private void Update()
    {
        if (coherence) return;

        state = power;

        if (!power)
        {
            StopMovement();
        } else
        {
            AlternatingMovement();
        }
    }

    private void StopMovement()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObstacle(minScale, minPos));
    }

    private void AlternatingMovement()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObstacle(maxScale, maxPos));
    }

    private IEnumerator ScaleObstacle(Vector3 scale, Vector3 position)
    {
        Vector3 startScale = model.localScale;
        Vector3 endScale = scale;

        Vector3 startPos = model.localPosition;
        Vector3 endPos = position;

        float time = scalingTime;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;

            model.localScale = Vector3.Lerp(startScale, endScale, scalingCurve.Evaluate(t / time));
            model.localPosition = Vector3.Lerp(startPos, endPos, scalingCurve.Evaluate(t / time));

            boxCollider.size = Vector3.Lerp(startScale, endScale, scalingCurve.Evaluate(t / time));
            boxCollider.center = Vector3.Lerp(startPos, endPos, scalingCurve.Evaluate(t / time));

            yield return null;
        }

        model.localScale = endScale;
        model.localPosition = position;
    }
}
