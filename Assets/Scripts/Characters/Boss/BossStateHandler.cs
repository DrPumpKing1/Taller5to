using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateHandler : MonoBehaviour
{
    public static BossStateHandler Instance;

    [Header("States")]
    [SerializeField] private State bossState;
    public  enum State { Rest, StartingPhase, OnPhase, Defeated }

    public State BossState => bossState;

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        HandleBossState();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossStateHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandleBossState()
    {

    }
}
