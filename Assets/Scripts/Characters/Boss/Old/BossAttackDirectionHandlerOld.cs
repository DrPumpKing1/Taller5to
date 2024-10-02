using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BossAttackDirectionHandlerOld : MonoBehaviour
{
    public static BossAttackDirectionHandlerOld Instance { get; private set; }

    [Header("Settings")]
    [SerializeField, Range(2f, 4f)] private float timeNotAttackedThreshold;

    [Header("Available Direction")]
    [SerializeField] private List<AttackDirection> attackDirections;

    [Serializable]
    public class AttackDirection
    {
        public Direction direction;
        [Range(0f, 360f)] public float upperAngleLimit;
        [Range(0f, 360f)] public float lowerAngleLimit;
        public Vector2 vectorizedDirection;
        public float timeNotAttacked;
    }

    public enum Direction { Front, Back, Left, Right }

    private List<AttackDirection> currentAttackDirections = new List<AttackDirection>();
    public List<AttackDirection> CurrentAttackDirections => currentAttackDirections;

    [Serializable]
    public class HitDirection
    {
        public Direction direction;
        public Transform block;
    }

    public class OnBossBlockEventArgs : EventArgs
    {
        public Direction direction;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleTimesNotAttacked();
        UpdateCurrentAttackDirections();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossAttackDirectionHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        ResetAllTimesNotAttacked();
    }

    private void HandleTimesNotAttacked()
    {
        foreach (AttackDirection attackDirection in attackDirections)
        {
            attackDirection.timeNotAttacked += Time.deltaTime;
        }
    }

    private void UpdateCurrentAttackDirections()
    {
        foreach (AttackDirection attackDirection in attackDirections)
        {
            if (currentAttackDirections.Contains(attackDirection) && attackDirection.timeNotAttacked >= timeNotAttackedThreshold) currentAttackDirections.Remove(attackDirection);
            if (!currentAttackDirections.Contains(attackDirection) && attackDirection.timeNotAttacked < timeNotAttackedThreshold) currentAttackDirections.Add(attackDirection);
        }
    }

    private void ResetAllTimesNotAttacked()
    {
        foreach(AttackDirection attackDirection in attackDirections)
        {
            attackDirection.timeNotAttacked = 0f;
        }
    }

    private void ResetTimeNotAttacked(AttackDirection attackDirection) => attackDirection.timeNotAttacked = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out SignalProjectile signalProjectile))
        {
            CheckHit(other.transform.position);
        }
    }

    private void CheckHit(Vector3 point)
    {
        float hitAngle = CalculateHitAngle(point);
        AttackDirection attackDirection = GetAttackDirection(hitAngle);

        if (attackDirection == null) return;

        ResetTimeNotAttacked(attackDirection);
    }

    private float CalculateHitAngle(Vector3 point) 
    {
        Vector3 hitVector = (point - transform.position).normalized;
        float hitAngle =  Mathf.Atan2(hitVector.z, hitVector.x) * Mathf.Rad2Deg;

        if (hitAngle < 0) hitAngle += 360f;

        return hitAngle;
    }

    private AttackDirection GetAttackDirection(float hitAngle)
    {
        foreach (AttackDirection attackDirection in attackDirections)
        {
            if (attackDirection.lowerAngleLimit > attackDirection.upperAngleLimit)
            {
                if ((attackDirection.upperAngleLimit >= hitAngle && hitAngle >= 0) || (360f >= hitAngle && hitAngle >= attackDirection.lowerAngleLimit)) return attackDirection;
            }
            else if (attackDirection.upperAngleLimit >= hitAngle && hitAngle >= attackDirection.lowerAngleLimit) return attackDirection;
        }

        return null;
    }
}
