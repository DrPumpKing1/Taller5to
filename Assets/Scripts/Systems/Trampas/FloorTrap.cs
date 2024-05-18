using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FloorTrap : MonoBehaviour
{
    public enum States
    {
        Idle,
        Shaking,
        Destroyed
    }

    [Header("Trap Settings")]
    public float trapShaking;
    public float trapCriticalTime;
    public float trapReconstructionTime;

    [Header("Trap Properties")]
    [SerializeField] private States state;
    [SerializeField] private float _timer;

    private void Awake()
    {
        state = States.Idle;
        _timer = 0;
    }

    private void Update()
    {
        if(state != States.Idle) _timer -= Time.deltaTime;

        if (_timer > 0) return;

        else if (state == States.Shaking)
        {
            state = States.Destroyed;
            _timer = trapReconstructionTime;
        }
        else if (state == States.Destroyed)
        {
            state = States.Idle;
            _timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (state == States.Idle)
            {
                state = States.Shaking;
                _timer = trapShaking;
            }

            else if(state == States.Destroyed)
            {
                Destroy(collision.gameObject);
            }
        }
    }
        
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (state == States.Shaking && _timer > trapCriticalTime)
            {
                state = States.Idle;
                _timer = 0;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (state == States.Destroyed)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
