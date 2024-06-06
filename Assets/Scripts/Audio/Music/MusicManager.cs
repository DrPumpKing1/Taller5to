using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MusicPoolSO musicPoolSO;

    private AudioSource audioSource;

    private const string MENU_SCENE_NAME = "MainMenu";
    private const string GAMEPLAY_SCENE_NAME = "Gameplay";
    private const string CREDITS_SCENE_NAME = "Credits";

    private void OnEnable()
    {
        ScenesManager.OnSceneLoad += ScenesManager_OnSceneLoad;
    }

    private void OnDisable()
    {
        ScenesManager.OnSceneLoad -= ScenesManager_OnSceneLoad;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip music)
    {
        if (!music)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        if (audioSource.clip != music)
        {
            audioSource.Stop();
            audioSource.clip = music;
            audioSource.Play();
        }
    }

    private void HandleScenesMusicPlay(string sceneName)
    {
        switch (sceneName)
        {
            case GAMEPLAY_SCENE_NAME:
                PlayMusic(musicPoolSO.gamePlayMusic);
                Debug.Log("GameplayMusicPlay");
                break;
            case MENU_SCENE_NAME:
                PlayMusic(musicPoolSO.menuMusic);
                Debug.Log("MainMenuMusicPlay");
                break;
            case CREDITS_SCENE_NAME:
                PlayMusic(musicPoolSO.creditsMusic);
                Debug.Log("CredistMusicPlay");
                break;
        }
    }

    private void ScenesManager_OnSceneLoad(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        HandleScenesMusicPlay(e.sceneName);
    }
}
