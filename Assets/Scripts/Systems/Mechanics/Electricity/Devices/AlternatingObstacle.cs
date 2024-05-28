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
    [SerializeField] private float force;

    [Header("Device Control")]
    [SerializeField] private bool state;
    private bool power => electrode.Power >= Electrode.ACTIVATION_THRESHOLD;
    private bool coherence => state == power;

    private void Start()
    {
        minScale = obstacle.localScale;
        maxScale = new Vector3(obstacle.localScale.x, obstacle.localScale.y + extension, obstacle.localScale.z);
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
        StartCoroutine(ScaleObstacle(minScale));
    }

    private void AlternatingMovement()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObstacleEternal(maxScale));
    }

    private IEnumerator ScaleObstacle(Vector3 scale)
    {
        Vector3 startScale = obstacle.localScale;
        Vector3 endScale = scale;
        float time = scalingTime;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;

            obstacle.localScale = Vector3.Lerp(startScale, endScale, scalingCurve.Evaluate(t / time));

            yield return null;
        }

        obstacle.localScale = endScale;
    }

    private IEnumerator ScaleObstacleEternal(Vector3 scale)
    {
        Vector3 startScale = obstacle.localScale;
        Vector3 endScale = scale;
        float time = scalingTime;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;

            obstacle.localScale = Vector3.Lerp(startScale, endScale, scalingCurve.Evaluate(t / time));

            yield return null;
        }

        obstacle.localScale = endScale;

        if(scale == maxScale) StartCoroutine(ScaleObstacleEternal(minScale));
        else if(scale == minScale) StartCoroutine(ScaleObstacleEternal(maxScale));
    }
}
