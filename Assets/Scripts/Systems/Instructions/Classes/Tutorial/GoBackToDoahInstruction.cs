using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToDoahInstruction : LogInstruction
{
    protected override bool CheckCondition() => ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned.Count <= 0;
}
