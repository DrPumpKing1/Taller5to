using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnowledgeSourceColletcionVisualSettingsSO", menuName = "ScriptableObjects/Knowledge/KnowledgeSource/KnowledgeSourceCollectionVisualSettings")]

public class KnowledgeSourceCollectionVisualSettingsSO : ScriptableObject
{
    [Header("Knowledge Added UI Settings")]
    public Transform knowledgeAddedUIPrefab;
    public Vector3 instantiationPositionOffset;
    public Vector3 offsetBetweenInstantiatedUIs;

    [Header("VFX Settings")]
    public Transform knowledgeAddedVFXPrefab;
}
