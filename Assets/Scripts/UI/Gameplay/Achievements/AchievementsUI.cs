using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator achievementsUIAnimator;
    [Space]
    [SerializeField] private TextMeshProUGUI achievementNameText;
    [SerializeField] private Image achievementImage;
    [SerializeField] private Image achievementPanelImage;

    [Header("Settings")]
    [SerializeField, Range(2f, 10f)] private float timeShowingAchievement;

    [Header("States")]
    [SerializeField] private State state;

    private enum State { Hidden, ShowingIn, Showing, ShowingOut } 

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const float SHOW_TIME = 1.5f;
    private const float HIDE_TIME = 1.5f;

    private void OnEnable()
    {
        AchievementManager.OnAchievementAchieved += AchievementManager_OnAchievementAchieved;
    }

    private void OnDisable()
    {
        AchievementManager.OnAchievementAchieved -= AchievementManager_OnAchievementAchieved;
    }

    private void Start()
    {
        SetState(State.Hidden);
    }

    private void SetState(State state) => this.state = state;

    private void ShowPopUp()
    {
        achievementsUIAnimator.ResetTrigger(HIDE_TRIGGER);
        achievementsUIAnimator.SetTrigger(SHOW_TRIGGER);
    }
    private void HidePopUp()
    {
        achievementsUIAnimator.ResetTrigger(SHOW_TRIGGER);
        achievementsUIAnimator.SetTrigger(HIDE_TRIGGER);
    }

    private void SetAchievementName(string name) => achievementNameText.text = name;
    private void SetAchievementImage(Sprite sprite) => achievementImage.sprite = sprite;    

    private IEnumerator ShowAchievementCoroutine(AchievementSO achievementSO)
    {
        SetAchievementName(achievementSO.achievementName);
        SetAchievementImage(achievementSO.achievementSprite);

        SetState(State.ShowingIn);

        ShowPopUp();
        yield return new WaitForSecondsRealtime(SHOW_TIME);

        SetState(State.Showing);

        yield return new WaitForSecondsRealtime(timeShowingAchievement);

        SetState(State.ShowingOut);

        HidePopUp();

        yield return new WaitForSecondsRealtime(HIDE_TIME);

        SetState(State.Hidden);
    }

    private void AchievementManager_OnAchievementAchieved(object sender, AchievementManager.OnAchievementAchievedEventArgs e)
    {
        if (state != State.Hidden) return;
        StopAllCoroutines();
        StartCoroutine(ShowAchievementCoroutine(e.achievementSO));
    }
}
