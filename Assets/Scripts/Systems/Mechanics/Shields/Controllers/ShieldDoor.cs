using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDoor : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;
    [SerializeField] private Dialect dialect;
    
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

    private bool isWorthy;
    private GameObject player;
    private Coroutine doorMovement;

    public static event EventHandler<OnShieldDoorOpenEventArgs> OnShieldDoorOpen;

    public class OnShieldDoorOpenEventArgs : EventArgs
    {
        public Dialect dialect;
        public int id;
    }

    private void Start()
    {
        InitializeVariables();
    }

    void Update()
    {
        CheckVirtue();
        CheckProximity();
        ManageState();
    }

    private void InitializeVariables()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void ManageState()
    {
        if(state == isWorthy) return;
        
        if(!isNextToPlayer) return;

        state = !state;

        if (state) 
        {
            OnShieldDoorOpen?.Invoke(this, new OnShieldDoorOpenEventArgs { dialect = dialect, id = id });
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
        isWorthy = ShieldPiecesManager.Instance.HasCompletedShield(dialect);
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
