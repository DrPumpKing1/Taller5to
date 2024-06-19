using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private FootstepsPoolSO footstepsPoolSO;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private AudioSource audioSource;

    private void OnEnable()
    {
        PlayerHorizontalMovement.OnPlayerStopMoving += PlayerHorizontalMovement_OnPlayerStopMoving;
        PlayerHorizontalMovement.OnPlayerStartWalking += PlayerHorizontalMovement_OnPlayerStartWalking;
        PlayerHorizontalMovement.OnPlayerStartSprinting += PlayerHorizontalMovement_OnPlayerStartSprinting;
    }

    private void OnDisable()
    {
        PlayerHorizontalMovement.OnPlayerStopMoving -= PlayerHorizontalMovement_OnPlayerStopMoving;
        PlayerHorizontalMovement.OnPlayerStartWalking -= PlayerHorizontalMovement_OnPlayerStartWalking;
        PlayerHorizontalMovement.OnPlayerStartSprinting -= PlayerHorizontalMovement_OnPlayerStartSprinting;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void StopAudioSource()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    private void ReplaceAudioClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = null;

        if(!clip)
        {
            if (debug) Debug.Log($"The clip {clip.name} is null");
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    private void PlayerHorizontalMovement_OnPlayerStopMoving(object sender, System.EventArgs e)
    {
        StopAudioSource();
    }
    private void PlayerHorizontalMovement_OnPlayerStartWalking(object sender, System.EventArgs e)
    {
        ReplaceAudioClip(footstepsPoolSO.walking);
    }
    private void PlayerHorizontalMovement_OnPlayerStartSprinting(object sender, System.EventArgs e)
    {
        ReplaceAudioClip(footstepsPoolSO.sprinting);
    }
}
