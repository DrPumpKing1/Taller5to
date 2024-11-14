using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievementSO", menuName = "ScriptableObjects/Achievement")]
public class AchievementSO : ScriptableObject
{
    public int id;
    public string achievementName;
    public Sprite achievementSprite;
}
