using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomNameUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI roomNameText;

    [Header("Settings")]
    [SerializeField, Range(0, 2f)] private float timeToShowRoomNameStart;
    [SerializeField, Range(0, 2f)] private float timeToShowRoomNameRegular;
    [SerializeField, Range(2f, 5f)] private float timeShowingRoomName;
    [SerializeField, Range(0.5f, 2f)] private float transitionTime;
    [Space]
    [SerializeField] private bool enableRoomNameOnStart;

    [Header("States")]
    [SerializeField] private RoomNameState state;

    private enum RoomNameState { Hidden, FadingIn, Showing, FadingOut }

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string HIDDEN_ANIMATION_NAME = "RoomNameHidden";

    private const string STATUES_ROOM_NAME = "Statues Room";
    private const string MAIN_STATUE_ROOM_NAME = "Main Statue Room";
    private const string INITIATION_ROOM_NAME = "Initiation Room";
    private const string GARDEN_OF_OFFERINGS_NAME = "Garden Of Offerings";
    private const string HEALERS_ROOM_NAME = "Healer's Room";
    private const string WRITINGS_ROOM_NAME = "Writings Room";
    private const string RIIDA_ROOM_NAME = "<i>Riida's</i> Room";
    private const string STORYTELLER_ROOM_NAME = "Storyteller Room";
    private const string RAKITHU_CHAMBER_NAME = "Rakithu Chamber";
    private const string XOTARK_CHAMBER_NAME = "Xotark Chamber";
    private const string VYTHANU_CHAMBER_NAME = "Vythanu Chamber";
    private const string ZURRYTH_CHAMBER_NAME = "Zurryth Chamber";
    private const string EASTER_EGG_1_NAME = "Easter Egg 1";

    public static event EventHandler OnRoomNameShow;
    public static event EventHandler OnRoomNameHide;

    private void OnEnable()
    {
        RoomManager.OnStartBlockingViewColliders += RoomManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders += RoomManager_OnEnterBlockingViewColliders;

        LevelTitleUI.OnLevelTitleShow += LevelTitleUI_OnLevelTitleShow;
    }

    private void OnDisable()
    {
        RoomManager.OnStartBlockingViewColliders -= RoomManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders -= RoomManager_OnEnterBlockingViewColliders;

        LevelTitleUI.OnLevelTitleShow -= LevelTitleUI_OnLevelTitleShow;
    }

    private void Awake()
    {
        SetRoomNameState(RoomNameState.Hidden);
    }

    private void SetRoomNameState(RoomNameState state) => this.state = state;

    private void ShowRoomName()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideRoomName()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void CheckRoomNameToShow(RoomSubArea roomSubArea, bool isStart)
    {
        string roomName = "";

        switch (roomSubArea)
        {
            case RoomSubArea.StatuesRoom:
                roomName = STATUES_ROOM_NAME;
                break;
            case RoomSubArea.MainStatueRoom:
                roomName = MAIN_STATUE_ROOM_NAME;
                break;
            case RoomSubArea.InitiationRoom:
                roomName = INITIATION_ROOM_NAME;
                break;
            case RoomSubArea.GardenOfOfferings:
                roomName = GARDEN_OF_OFFERINGS_NAME;
                break;
            case RoomSubArea.HealersRoom:
                roomName = HEALERS_ROOM_NAME;
                break;
            case RoomSubArea.WritingsRoom:
                roomName = WRITINGS_ROOM_NAME;
                break;
            case RoomSubArea.RiidaRoom:
                roomName = RIIDA_ROOM_NAME;
                break;
            case RoomSubArea.StoryTellerRoom:
                roomName = STORYTELLER_ROOM_NAME;
                break;
            case RoomSubArea.RakithuChamber:
                roomName = RAKITHU_CHAMBER_NAME;
                break;
            case RoomSubArea.XotarkChamber:
                roomName = XOTARK_CHAMBER_NAME;
                break;
            case RoomSubArea.VythanuChamber:
                roomName = VYTHANU_CHAMBER_NAME;
                break;
            case RoomSubArea.ZurrythChamber:
                roomName = ZURRYTH_CHAMBER_NAME;
                break;
            case RoomSubArea.EasterEgg1:
                roomName = EASTER_EGG_1_NAME;
                break;
            default:
                return;
        }

        RoomNameShow(roomName, isStart);
    }

    private void RoomNameShow(string roomName, bool isStart)
    {
        StopAllCoroutines();
        StartCoroutine(RoomNameShowCoroutine(roomName, isStart));
    }

    private IEnumerator RoomNameShowCoroutine(string roomName, bool isStart)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(HIDDEN_ANIMATION_NAME) || animator.IsInTransition(0))
        {
            yield return HideCurrentRoomNameCoroutine();
        }

        float timeToShow;

        if (isStart) timeToShow = timeToShowRoomNameStart;
        else timeToShow = timeToShowRoomNameRegular;

        SetRoomNameState(RoomNameState.FadingIn);

        OnRoomNameShow?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(timeToShow);

        SetRoomNameText(roomName);

        SetRoomNameState(RoomNameState.Showing);

        ShowRoomName();

        yield return new WaitForSeconds(timeShowingRoomName);

        yield return StartCoroutine(HideCurrentRoomNameCoroutine());
    }

    private IEnumerator HideCurrentRoomNameCoroutine()
    {
        SetRoomNameState(RoomNameState.FadingOut);

        HideRoomName();

        yield return new WaitForSeconds(transitionTime);

        OnRoomNameHide?.Invoke(this, EventArgs.Empty);

        SetRoomNameState(RoomNameState.Hidden);
    }

    private void SetRoomNameText(string title) => roomNameText.text = title;
    private void ClearRoomNameText() => roomNameText.text = "";

    #region RoomManager Subcriptions
    private void RoomManager_OnStartBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersStartEventArgs e)
    {
        if (e.currentRoomVisibilityColliders.Count == 0) return;
        if (!enableRoomNameOnStart) return;

        CheckRoomNameToShow(e.currentRoomVisibilityColliders[0].RoomSubArea, true);
    }

    private void RoomManager_OnEnterBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersEnterEventArgs e)
    {
        if (e.newRoomVisibilityColliders.Count == 0) return;
        if (e.previousRoomVisibilityColliders[0].RoomSubArea == e.newRoomVisibilityColliders[0].RoomSubArea) return;

        CheckRoomNameToShow(e.newRoomVisibilityColliders[0].RoomSubArea, false);
    }
    #endregion

    #region LevelTitleUI Subscriptions
    private void LevelTitleUI_OnLevelTitleShow(object sender, EventArgs e)
    {
        if (state == RoomNameState.Hidden) return;
        if (state == RoomNameState.FadingOut) return;

        StopAllCoroutines();
        StartCoroutine(HideCurrentRoomNameCoroutine());
    }
    #endregion
}
