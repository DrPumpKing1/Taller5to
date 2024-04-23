using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewDialectWriting", menuName = "ScriptableObjects/Dialects/DialectWriting")]
public class DialectWritingSO : ScriptableObject
{
    public Dialect dialect;
    public Sprite writingImage;
}
