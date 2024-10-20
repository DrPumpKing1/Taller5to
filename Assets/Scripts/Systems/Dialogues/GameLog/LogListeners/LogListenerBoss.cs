using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerBoss : MonoBehaviour
{
    private void OnEnable()
    {
        //BOSS
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandler.OnBossAlmostDefeated += BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;

        //ANCIENT RELIC
        AncientRelic.OnAncientRelicCollected += AncientRelic_OnAncientRelicCollected;
    }


    private void OnDisable()
    {
        //BOSS
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandler.OnBossAlmostDefeated -= BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;

        //ANCIENT RELIC
        AncientRelic.OnAncientRelicCollected -= AncientRelic_OnAncientRelicCollected;
    }

    //BOSS
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        GameLogManager.Instance.Log($"Boss/PhaseChange/Start/Any");
        GameLogManager.Instance.Log($"Boss/PhaseChange/Start/{e.nextPhase}");
    }
    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        GameLogManager.Instance.Log($"Boss/PhaseChange/End/Any");
        GameLogManager.Instance.Log($"Boss/PhaseChange/End/{e.nextPhase}");
    }

    private void BossStateHandler_OnBossAlmostDefeated(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Boss/AlmostDefeated");

    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Boss/Defeated");
    private void AncientRelic_OnAncientRelicCollected(object sender, System.EventArgs e) => GameLogManager.Instance.Log("Events/AncientRelicCollected");
}
