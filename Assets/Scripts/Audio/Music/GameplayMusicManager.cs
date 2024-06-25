using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MusicPoolSO musicPoolSO;

    [Header("Debug")]
    [SerializeField] private AudioClip currentGameplayMusic;

    private void OnEnable()
    {
        RoomManager.OnStartBlockingViewColliders += RoomManager_OnStartBlockingViewColliders;
    }

    private void OnDisable()
    {
        RoomManager.OnStartBlockingViewColliders -= RoomManager_OnStartBlockingViewColliders;
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

        if (currentGameplayMusic == musicToPlay) return;

        MusicManager.Instance.PlayMusic(musicToPlay);
        currentGameplayMusic = musicToPlay;

        Debug.Log($"GameplayMusicPlay : {musicToPlay}");
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
}
