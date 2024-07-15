using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguesAudioManager : MonoBehaviour
{
    public static DialoguesAudioManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private AudioClip currentSentenceClip;
    [SerializeField] private bool debug;

    private AudioSource audioSource;

    private void OnEnable()
    {
        DialogueManager.OnSentencePlay += DialogueManager_OnSentencePlay;
        DialogueManager.OnSentenceSkip += DialogueManager_OnSentenceSkip;
    }

    private void OnDisable()
    {
        DialogueManager.OnSentencePlay -= DialogueManager_OnSentencePlay;
        DialogueManager.OnSentenceSkip -= DialogueManager_OnSentenceSkip;
    }

    private void Awake()
    {
        SetSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Debug.LogWarning("There is more than one DialoguesAudioManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void HandleSentenceAudio(AudioClip sentence)
    {
        if (!sentence)
        {
            ConcludeSentenceAudio();
            if (debug) Debug.Log("There's not a sentence clip to play!");
        }
        else
        {
            PlaySentenceAudio(sentence);
        }
    }

    private void PlaySentenceAudio(AudioClip dialogue)
    {
        audioSource.Stop();
        audioSource.clip = dialogue;
        audioSource.Play();
        currentSentenceClip = audioSource.clip;
    }

    public void ConcludeSentenceAudio()
    {
        audioSource.Stop();
        audioSource.clip = null;
        currentSentenceClip = null;
    }

    #region DialogueManager Subscriptions
    private void DialogueManager_OnSentencePlay(object sender, DialogueManager.OnSentencePlayEventArgs e)
    {
        HandleSentenceAudio(e.sentence.audioClip);
    }
    private void DialogueManager_OnSentenceSkip(object sender, DialogueManager.OnSentenceSkipEventArgs e)
    {
        ConcludeSentenceAudio();
    }
    #endregion
}
