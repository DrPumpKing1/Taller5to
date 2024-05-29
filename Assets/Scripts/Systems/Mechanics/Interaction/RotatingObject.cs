using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public float detectionRadius;
    public float rotationSpeed;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        CheckPlayer();
        RotateObject();
    }

    private void CheckPlayer()
    {
        if(player == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            GetObject();
        }
    }

    private void RotateObject()
    {
        transform.localRotation *= Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void GetObject()
    {
        //Things!
        GameLog.Log("Interaction/VyrxLearnObject");
        GameLog.Log("Interaction/GrabGems");
        Destroy(gameObject);
    }
}
