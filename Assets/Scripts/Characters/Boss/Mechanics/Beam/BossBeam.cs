using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossBeam : MonoBehaviour
{
    public static BossBeam Instance {  get; private set; }

    public static event EventHandler<OnBeamEventArgs> OnBeamStart;
    public static event EventHandler<OnBeamEventArgs> OnBeamEnd;

    public class OnBeamEventArgs
    {
        public BossPhase bossPhase;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        
        //Test();
        
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnBeamStart?.Invoke(this, new OnBeamEventArgs { bossPhase = BossPhase.Phase3 });
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            OnBeamEnd?.Invoke(this, new OnBeamEventArgs { bossPhase = BossPhase.Phase3 });
        }
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossBeam, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
