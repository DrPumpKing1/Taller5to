using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    private enum State { Idle, TransitionIn, TransitionOut }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetSceneState(State state) => this.state = state;

    private bool CanChangeScene() => state == State.Idle;

    public void SimpleReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SimpleLoadTargetScene(currentSceneName);
    }

    public void SimpleLoadTargetScene(string sceneName)
    {
        if (!CanChangeScene()) return;
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame() => Application.Quit();
}
