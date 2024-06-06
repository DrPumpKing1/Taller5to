using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingObstacle : MonoBehaviour
{
    [Header("Electrical Component")]
    [SerializeField] private Electrode electrode;

    [Header("Device Settings")]
    [SerializeField] private Transform obstacle;
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
        minScale = obstacle.localScale;
        maxScale = new Vector3(obstacle.localScale.x, obstacle.localScale.y, obstacle.localScale.z + extension);

        minPos = obstacle.localPosition;
        maxPos = new Vector3(obstacle.localPosition.x, obstacle.localPosition.y, obstacle.localPosition.z + extension/2);
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
        Vector3 startScale = obstacle.localScale;
        Vector3 endScale = scale;

        Vector3 startPos = obstacle.localPosition;
        Vector3 endPos = position;

        float time = scalingTime;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;

            obstacle.localScale = Vector3.Lerp(startScale, endScale, scalingCurve.Evaluate(t / time));
            obstacle.localPosition = Vector3.Lerp(startPos, endPos, scalingCurve.Evaluate(t / time));

            yield return null;
        }

        obstacle.localScale = endScale;
        obstacle.localPosition = position;
    }
}
