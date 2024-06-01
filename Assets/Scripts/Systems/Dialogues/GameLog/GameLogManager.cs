using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogManager : MonoBehaviour
{
    public static event Action OnLogAdd;

    public static GameLogManager Instance { get; private set; }

    [Header("Log")] 
    [SerializeField] private List<GameplayAction> gameLog;
    [SerializeField] private bool enableGameLog;

    public List<GameplayAction> GameLog => gameLog;

    [Serializable]
    public struct GameplayAction
    {
        public float time;
        public string log;
    }
     
    private void Awake()
    {
        SetSingleton();      
        InitializeLog();
    }

    private void Start()
    {
        Log("GameFlow/Start");
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameLog instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeLog()
    {
        gameLog = new List<GameplayAction>();
    }

    public void Log(string log)
    {
        if (!enableGameLog) return;

        Instance.gameLog.Add(new GameplayAction
        {
            time = Time.time,
            log = log
        });
        
        OnLogAdd?.Invoke();
    }
}
