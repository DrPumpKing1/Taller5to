using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MusicPoolSO musicPoolSO;

    [Header("Gameplay Music Transition Settings")]
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float muteTime;

    [Header("Debug")]
    [SerializeField] private AudioClip currentGameplayMusic;

    private void OnEnable()
    {
        RoomManager.OnStartBlockingViewColliders += RoomManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders += RoomManager_OnEnterBlockingViewColliders;

        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;
    }

    private void OnDisable()
    {
        RoomManager.OnStartBlockingViewColliders -= RoomManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders -= RoomManager_OnEnterBlockingViewColliders;

        ShieldPiecesManager.OnShieldPieceCollected -= ShieldPiecesManager_OnShieldPieceCollected;
    }

    private void CheckStartMusicToPlay(Level level)
    {
        AudioClip musicToPlay = musicPoolSO.zurryth0;

        switch (level)
        {
            case Level.Tutorial:
                musicToPlay = CheckZurrythMusicToPlay();
                break;
            case Level.Lobby:
                musicToPlay = CheckLobbyMusicToPlay();
                break;
            case Level.Level1:
                musicToPlay = CheckRakithuMusicToPlay();
                break;
            case Level.Level2:
                musicToPlay = CheckXotarkMusicToPlay();
                break;
            case Level.Level3:
                musicToPlay = CheckVyhtanuMusicToPlay();
                break;
            case Level.Boss:
                musicToPlay = CheckBossMusicToPlay();
                break;
            default:
                break;
        }

        PlayGameplayMusic(musicToPlay);
    }

    private void CheckRoomChangeMusicToPlay(Level level)
    {
        AudioClip musicToPlay = musicPoolSO.zurryth0;

        switch (level)
        {
            case Level.Tutorial:
                musicToPlay = CheckZurrythMusicToPlay();
                break;
            case Level.Lobby:
                musicToPlay = CheckLobbyMusicToPlay();
                break;
            case Level.Level1:
                musicToPlay = CheckRakithuMusicToPlay();
                break;
            case Level.Level2:
                musicToPlay = CheckXotarkMusicToPlay();
                break;
            case Level.Level3:
                musicToPlay = CheckVyhtanuMusicToPlay();
                break;
            case Level.Boss:
                musicToPlay = CheckBossMusicToPlay();
                break;
            default:
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

        Debug.Log($"GameplayMusicPlay : {gameplayMusic}");
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
        int shieldNumber = ShieldPiecesManager.Instance.GetNumberOfPiecesByDialect(Dialect.Xotark);

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
    }
    #endregion


    private void RoomManager_OnStartBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersStartEventArgs e)
    {
        if (e.currentRoomVisibilityColliders.Count == 0) return;

        CheckStartMusicToPlay(e.currentRoomVisibilityColliders[0].Level);
    }

    private void RoomManager_OnEnterBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersEnterEventArgs e)
    {
        if (e.newRoomVisibilityColliders.Count == 0) return;
        if (e.previousRoomVisibilityColliders[0].Level == e.newRoomVisibilityColliders[0].Level) return;

        CheckRoomChangeMusicToPlay(e.newRoomVisibilityColliders[0].Level);
    }

    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e)
    {
        CheckShieldCollectedMusicToPlay(e.shieldPieceSO.dialect);
    }
}
