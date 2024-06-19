using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFootstepsPoolSO", menuName = "ScriptableObjects/Audio/CustomSFX/FootstepsPool")]

public class FootstepsPoolSO : ScriptableObject
{
    [Header("Footsteps")]
    public AudioClip walking;
    public AudioClip sprinting;
}
