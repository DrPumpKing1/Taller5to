using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtueDoor : MonoBehaviour
{
    [Header("Dialect")]
    public Dialect dialect;
    private bool isVirtue;
    
    [Header("Device Settings")]
    [SerializeField] private Transform gateTransform;
    [SerializeField] private Transform restPosition;
    [SerializeField] private Transform powerPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private AnimationCurve moveCurve;

    [Header("Proximity Check")] 
    [SerializeField] private Transform doorCenter;
    [SerializeField] private float proximityRadius;
    private bool isNextToPlayer;
    
    [Header("State")]
    [SerializeField] private bool state;
    private GameObject player;
    private Coroutine doorMovement;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        CheckVirtue();
        CheckProximity();
        ManageState();
    }

    private void ManageState()
    {
        if(state == isVirtue) return;
        
        if(!isNextToPlayer) return;

        state = !state;

        if (state)
        {
            GameLog.Log("Virtue/ShowDignity");
        }
        
        TriggerMovement(state);
    }
    
    private void CheckProximity()
    {
        if (player == null)
        {
            isNextToPlayer = false;
            return;
        }
        
        isNextToPlayer = Vector3.Distance(doorCenter.position, player.transform.position) <= proximityRadius;
    }

    private void CheckVirtue()
    {
        isVirtue = ShieldPiecesManager.Instance.HasCompletedShield(dialect);
    }
    
    private void TriggerMovement(bool state)
    {
        if(doorMovement != null)StopCoroutine(doorMovement);
        doorMovement = StartCoroutine(MoveToPosition(state ? powerPosition.position : restPosition.position));
    }

    private IEnumerator MoveToPosition(Vector3 position)
    {
        Vector3 startPosition = gateTransform.position;
        Vector3 endPosition = position;
        float distance = Vector3.Distance(startPosition, endPosition);
        float time = distance / moveSpeed;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;

            gateTransform.position = Vector3.Lerp(startPosition, endPosition, moveCurve.Evaluate(t / time));

            yield return null;
        }

        gateTransform.position = endPosition;
    }
}
