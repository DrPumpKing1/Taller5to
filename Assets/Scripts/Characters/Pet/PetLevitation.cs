using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetLevitation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform transformToLevitate;
    [SerializeField] private Transform refferenceTransform;

    [Header("Enabler")]
    [SerializeField] private bool levitationEnabled;

    [Header("Settings")]
    [SerializeField, Range(0f, 2f)] private float amplitudeX;
    [SerializeField, Range(0f, 2f)] private float frequencyX;
    [Space]
    [SerializeField, Range(0f, 2f)] private float amplitudeY;
    [SerializeField, Range(0f, 2f)] private float frequencyY;
    [Space]
    [SerializeField, Range(0f, 2f)] private float amplitudeZ;
    [SerializeField, Range(0f, 2f)] private float frequencyZ;

    private void Update()
    {
        if (!levitationEnabled) return;
        HandleLevitation();
    }

    private void HandleLevitation()
    {
        float x = refferenceTransform.position.x + amplitudeX * Mathf.Sin(Time.time * frequencyX);
        float y = refferenceTransform.position.y + amplitudeY * Mathf.Sin(Time.time * frequencyY);
        float z = refferenceTransform.position.z + amplitudeZ * Mathf.Sin(Time.time * frequencyZ);

        transformToLevitate.position = new Vector3(x,y,z);
    }
}
