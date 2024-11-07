using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MusicPoolSO musicPoolSO;

    [Header("Gameplay Music Transition Settings")]
    [SerializeField,Range(0.1f,2f)]  private float fadeOutTime;
    [SerializeField, Range(0.1f, 2f)] private float fadeInTime;
    [SerializeField, Range(0.1f, 2f)] private float muteTime;

    [Header("Cinematic Gameplay Music Fade Settings")]
    [SerializeField, Range(0.1f, 2f)] private float fadeOutTimeCinematics;
    [SerializeField, Range(0.1f, 2f)] private float fadeInTimeCinematics;

    [Header("Debug")]
    [SerializeField] private AudioClip currentGameplayMusic;

    private void OnEnable()
    {
        RoomManager.OnStartBlockingViewColliders += RoomManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders += RoomManager_OnEnterBlockingViewColliders;

        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;

        CinematicsManager.OnCinematicStart += CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEnd += CinematicsManager_OnCinematicEnd;

        BossStateHandlerOld.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    

    private void OnDisable()
    {
        RoomManager.OnStartBlockingViewColliders -= RoomManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders -= RoomManager_OnEnterBlockingViewColliders;

        ShieldPiecesManager.OnShieldPieceCollected -= ShieldPiecesManager_OnShieldPieceCollected;

        CinematicsManager.OnCinematicStart -= CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEnd -= CinematicsManager_OnCinematicEnd;

        BossStateHandlerOld.OnBossDefeated -= BossStateHandler_OnBossDefeated;
    }

    private void CheckStartMusicToPlay(MusicLevel level)
    {
        AudioClip musicToPlay = musicPoolSO.zurryth0;

        switch (level)
        {
            case MusicLevel.Tutorial:
                musicToPlay = CheckZurrythMusicToPlay();
                break;
            case MusicLevel.Lobby:
                musicToPlay = CheckLobbyMusicToPlay();
                break;
            case MusicLevel.Level1:
                musicToPlay = CheckRakithuMusicToPlay();
                break;
            case MusicLevel.Level2:
                musicToPlay = CheckXotarkMusicToPlay();
                break;
            case MusicLevel.Level3:
                musicToPlay = CheckVyhtanuMusicToPlay();
                break;
            case MusicLevel.Boss:
                musicToPlay = CheckBossMusicToPlay();
                break;
            case MusicLevel.Showcase:
                musicToPlay = CheckShowcaseMusicToPlay();
                break;
            default:
                break;
        }

        PlayGameplayMusic(musicToPlay);
    }

    private void CheckRoomChangeMusicToPlay(MusicLevel level)
    {
        AudioClip musicToPlay = musicPoolSO.zurryth0;

        switch (level)
        {
            case MusicLevel.Tutorial:
                musicToPlay = CheckZurrythMusicToPlay();
                break;
            case MusicLevel.Lobby:
                musicToPlay = CheckLobbyMusicToPlay();
                break;
            case MusicLevel.Level1:
                musicToPlay = CheckRakithuMusicToPlay();
                break;
            case MusicLevel.Level2:
                musicToPlay = CheckXotarkMusicToPlay();
                break;
            case MusicLevel.Level3:
                musicToPlay = CheckVyhtanuMusicToPlay();
                break;
            case MusicLevel.Boss:
                musicToPlay = CheckBossMusicToPlay();
                break;
            case MusicLevel.Showcase:
                musicToPlay = CheckShowcaseMusicToPlay();
                break;
            default:
                Debug.Log("Playing Default Music");
                break;
        }

        FadeTransitionGameplayMusic(musicToPlay);
    }

    private void CheckShieldCollectedMusicToPlay(Dialect dialect)
    {
        AudioClip musicToPlay = musicPoolSO.zurryth0;

        switch (dialect)
        {
            case Dialect.Zurryth:
                musicToPlay = CheckZurrythMusicToPlay();
                break;
            case Dialect.Rakithu:
                musicToPlay = CheckRakithuMusicToPlay();
                break;
            case Dialect.Xotark:
                musicToPlay = CheckXotarkMusicToPlay();
                break;
            case Dialect.Vythanu:
                musicToPlay = CheckVyhtanuMusicToPlay();
                break;
            default:
                break;
        }

        FadeTransitionGameplayMusic(musicToPlay);
    }

    private void PlayGameplayMusic(AudioClip gameplayMusic)
    {
        MusicManager.Instance.PlayMusic(gameplayMusic);
        currentGameplayMusic = gameplayMusic;

        Debug.Log($"GameplayMusicPlay: {gameplayMusic}");
    }

    private void FadeTransitionGameplayMusic(AudioClip gameplayMusic)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTransitionGameplayMusicCoroutine(gameplayMusic));
    }

    private IEnumerator FadeTransitionGameplayMusicCoroutine(AudioClip gameplayMusic)
    {
        yield return StartCoroutine(MusicFadeManager.Instance.FadeOutMusicCoroutine(fadeOutTime));

        yield return new WaitForSecondsRealtime(muteTime);

        PlayGameplayMusic(gameplayMusic);

        yield return StartCoroutine(MusicFadeManager.Instance.FadeInMusicCoroutine(fadeInTime));

    }


    #region DialectMusic
    private AudioClip CheckZurrythMusicToPlay()
    {
        int shieldNumber = ShieldPiecesManager.Instance.GetNumberOfPiecesByDialect(Dialect.Zurryth);

        switch (shieldNumber)
        {
            case 0:
                return musicPoolSO.zurryth0;
            case 1:
                return musicPoolSO.zurryth1;
            default:
                break;
        }

        return musicPoolSO.zurryth0;
    }
    private AudioClip CheckRakithuMusicToPlay()
    {
        int shieldNumber = ShieldPiecesManager.Instance.GetNumberOfPiecesByDialect(Dialect.Rakithu);

        switch (shieldNumber)
        {
            case 0:
                return musicPoolSO.rakithu0;
            case 1:
                return musicPoolSO.rakithu1;
            case 2:
                return musicPoolSO.rakithu2;
            case 3:
                return musicPoolSO.rakithu3;
            case 4:
                return musicPoolSO.rakithu4;
            default:
                break;
        }

        return musicPoolSO.rakithu0;
    }
    private AudioClip CheckXotarkMusicToPlay()
    {
        int shieldNumber = ShieldPiecesManager.Instance.GetNumberOfPiecesByDialect(Dialect.Xotark);

        switch (shieldNumber)
        {
            case 0:
                return musicPoolSO.xotark0;
            case 1:
                return musicPoolSO.xotark1;
            case 2:
                return musicPoolSO.xotark2;
            case 3:
                return musicPoolSO.xotark3;
            case 4:
                return musicPoolSO.xotark4;
            default:
                break;
        }

        return musicPoolSO.xotark0;
    }
    private AudioClip CheckVyhtanuMusicToPlay()
    {
        int shieldNumber = ShieldPiecesManager.Instance.GetNumberOfPiecesByDialect(Dialect.Vythanu);

        switch (shieldNumber)
        {
            case 0:
                return musicPoolSO.vythanu0;
            case 1:
                return musicPoolSO.vythanu1;
            case 2:
                return musicPoolSO.vythanu2;
            case 3:
                return musicPoolSO.vythanu3;
            case 4:
                return musicPoolSO.vythanu4;
            default:
                break;
        }

        return musicPoolSO.vythanu0;
    }
    #endregion

    #region LobbyMusic
    private AudioClip CheckLobbyMusicToPlay()
    {
        return musicPoolSO.lobby;
    }
    #endregion

    #region BossMusic
    private AudioClip CheckBossMusicToPlay()
    {
        return musicPoolSO.boss;

        /*
        if (BossStateHandler.Instance.BossDefeated) return musicPoolSO.afterBoss;
        else return musicPoolSO.boss;
        */
    }
    #endregion

    #region ShowcaseMusic
    private AudioClip CheckShowcaseMusicToPlay()
    {
        return musicPoolSO.showcase;
    }
    #endregion

    #region RoomManager Subscriptions
    private void RoomManager_OnStartBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersStartEventArgs e)
    {
        if (e.currentRoomVisibilityColliders.Count == 0) return;

        CheckStartMusicToPlay(e.currentRoomVisibilityColliders[0].MusicLevel);
    }

    private void RoomManager_OnEnterBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersEnterEventArgs e)
    {
        if (e.newRoomVisibilityColliders.Count == 0) return;
        if (e.previousRoomVisibilityColliders[0].MusicLevel == e.newRoomVisibilityColliders[0].MusicLevel) return;

        CheckRoomChangeMusicToPlay(e.newRoomVisibilityColliders[0].MusicLevel);
    }
    #endregion

    #region ShieldPiecesManagerSubscriptions
    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e)
    {
        CheckShieldCollectedMusicToPlay(e.shieldPieceSO.dialect);
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        FadeTransitionGameplayMusic(musicPoolSO.afterBoss);
    }
    #endregion

    #region CinematicsManagerSubscriptions
    private void CinematicsManager_OnCinematicStart(object sender, CinematicsManager.OnCinematicEventArgs e)
    {
        StartCoroutine(MusicFadeManager.Instance.FadeOutMusicCoroutine(fadeOutTimeCinematics));
    }

    private void CinematicsManager_OnCinematicEnd(object sender, CinematicsManager.OnCinematicEventArgs e)
    {
        StartCoroutine(MusicFadeManager.Instance.FadeInMusicCoroutine(fadeInTimeCinematics));
    }
    #endregion
}
