using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTitleUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI levelTitleText;

    [Header("Settings")]
    [SerializeField, Range(0, 2f)] private float timeToShowLevelTitleStart;
    [SerializeField, Range(0, 2f)] private float timeToShowLevelTitleRegular;
    [SerializeField,Range(2f,5f)] private float timeShowingLevelTitle;
    [SerializeField, Range(0.5f, 2f)] private float transitionTime;

    [Header("States")]
    [SerializeField] private LevelTitleState state;

    private enum LevelTitleState { Hidden, FadingIn, Showing, FadingOut}

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string HIDDEN_ANIMATION_NAME = "LevelTitleHidden";

    private const string EXTERIOR_TITLE = "Aesyth Planet";
    private const string TUTORIAL_TITLE = "Divine Liberator Entry";
    private const string LOBBY_TITLE = "Chamber of Cultural Passages";
    private const string LEVEL1_TITLE = "Sanctuary of <i>Ka'hoshita</i>";
    private const string LEVEL2_TITLE = "Spiritual Wisdom Center";
    private const string LEVEL3_TITLE = "Rest of the Lost Souls";
    private const string BOSS_TITLE = "Warden's Room";
    private const string FINAL_ROOM_TITLE = "<i>Katryssa Midrita</i> Sacred Chamber";

    public static event EventHandler OnLevelTitleShow;
    public static event EventHandler OnLevelTitleHide;

    private void OnEnable()
    {
        RoomManager.OnStartBlockingViewColliders += RoomManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders += RoomManager_OnEnterBlockingViewColliders;
    }

    private void OnDisable()
    {
        RoomManager.OnStartBlockingViewColliders -= RoomManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders -= RoomManager_OnEnterBlockingViewColliders;
    }

    private void Awake()
    {
        SetLevelTitleState(LevelTitleState.Hidden);
    }

    private void SetLevelTitleState(LevelTitleState state) => this.state = state;

    private void ShowLevelTitle()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideLevelTitle()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void CheckLevelTitleToShow(TitleLevel titleLevel, bool isStart)
    {
        string levelTitle = "";

        switch (titleLevel)
        {
            case TitleLevel.Exterior:
                levelTitle = EXTERIOR_TITLE;
                break;
            case TitleLevel.Tutorial:
                levelTitle = TUTORIAL_TITLE;
                break;
            case TitleLevel.Lobby:
                levelTitle = LOBBY_TITLE;
                break;
            case TitleLevel.Level1:
                levelTitle = LEVEL1_TITLE;
                break;
            case TitleLevel.Level2:
                levelTitle = LEVEL2_TITLE;
                break;
            case TitleLevel.Level3:
                levelTitle = LEVEL3_TITLE;
                break;
            case TitleLevel.Boss:
                levelTitle = BOSS_TITLE;
                break;
            case TitleLevel.FinalRoom:
                levelTitle = FINAL_ROOM_TITLE;
                break;
            default:
                return;
        }

        LevelTitleShow(levelTitle, isStart);
    }

    private void LevelTitleShow(string levelTitle, bool isStart)
    {
        StopAllCoroutines();
        StartCoroutine(LevelTitleShowCoroutine(levelTitle, isStart));
    }

    private IEnumerator LevelTitleShowCoroutine(string levelTitle, bool isStart)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(HIDDEN_ANIMATION_NAME) || animator.IsInTransition(0))
        {
            yield return HideCurrentLevelTitleCoroutine();
        }

        float timeToShow;

        if (isStart) timeToShow = timeToShowLevelTitleStart;
        else timeToShow = timeToShowLevelTitleRegular;

        SetLevelTitleState(LevelTitleState.FadingIn);

        OnLevelTitleShow?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(timeToShow);

        SetLevelTitleText(levelTitle);

        SetLevelTitleState(LevelTitleState.Showing);

        ShowLevelTitle();

        yield return new WaitForSeconds(timeShowingLevelTitle);

        HideLevelTitle();

        OnLevelTitleHide?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator HideCurrentLevelTitleCoroutine()
    {
        SetLevelTitleState(LevelTitleState.FadingOut);

        HideLevelTitle();

        yield return new WaitForSeconds(transitionTime);

        OnLevelTitleHide?.Invoke(this, EventArgs.Empty);

        SetLevelTitleState(LevelTitleState.Hidden);
    }

    private void SetLevelTitleText(string title) => levelTitleText.text = title;
    private void ClearLevelTitleText() => levelTitleText.text = "";

    private void RoomManager_OnStartBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersStartEventArgs e)
    {
        if (e.currentRoomVisibilityColliders.Count == 0) return;

        CheckLevelTitleToShow(e.currentRoomVisibilityColliders[0].TitleLevel, true);
    }

    private void RoomManager_OnEnterBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersEnterEventArgs e)
    {
        if (e.newRoomVisibilityColliders.Count == 0) return;
        if (e.previousRoomVisibilityColliders[0].TitleLevel == e.newRoomVisibilityColliders[0].TitleLevel) return;

        CheckLevelTitleToShow(e.newRoomVisibilityColliders[0].TitleLevel, false);
    }
}
