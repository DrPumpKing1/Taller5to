using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public float rotationSpeed;

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        transform.localRotation *= Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
