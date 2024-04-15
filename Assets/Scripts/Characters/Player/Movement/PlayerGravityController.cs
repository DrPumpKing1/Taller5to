using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField] private float gravityMultiplier = 1f;

    //public float GetGravityMultiplier() => gravityMultiplier;
    //public float GetGravity() => Physics.gravity.y * gravityMultiplier;
}
