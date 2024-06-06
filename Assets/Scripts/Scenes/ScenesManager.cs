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

    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoad;

    public class OnSceneLoadEventArgs : EventArgs
    {
        public string sceneName;
    }

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

    private void Start()
    {
        OnSceneLoad?.Invoke(this, new OnSceneLoadEventArgs { sceneName = SceneManager.GetActiveScene().name });
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
        LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        OnSceneLoad?.Invoke(this, new OnSceneLoadEventArgs { sceneName = sceneName });
    }

    public void QuitGame() => Application.Quit();
}
