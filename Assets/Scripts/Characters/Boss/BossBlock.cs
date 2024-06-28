using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossBlock : MonoBehaviour
{
    [Header("Available Direction")]
    [SerializeField] private List<AttackDirection> attackDirections;

    [Serializable]
    public class AttackDirection
    {
        public Direction direction;
        [Range(0f, 360f)] public float upperAngleLimit;
        [Range(0f, 360f)] public float lowerAngleLimit;
        public Vector2 vectorizedDirection;
    }

    public enum Direction {Front, Back, Left, Right}

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out SignalProjectile signalProjectile))
        {
            CheckHit(collision.GetContact(0).point);
        }
    }

    private void CheckHit(Vector3 point)
    {
        float hitAngle = CalculateHitAngle(point);
        AttackDirection attackDirection = GetAttackDirection(hitAngle);

        Debug.Log(attackDirection.direction);
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
