using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance { get; private set; }

    private enum State {OnGameplay, OnUI, OnUIProgress }
    private State state;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetGameState(State.OnGameplay);
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
            Debug.LogWarning("There is more than one GameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetGameState(State state) => this.state = state;
}
