using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossCardinalBlock : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Direction> availableDirections;
    [SerializeField] private bool randomizeDirection;
    [Space]
    [SerializeField] private List<Direction> currentBlockingDirections;
    public enum Direction {Front, Back, Left, Right}

    public static event EventHandler<OnBossBlockEventArgs> OnBossBlock;

    public class OnBossBlockEventArgs : EventArgs
    {
        public Direction direction;
    }

    private void OnEnable()
    {
        BossPhaseHandler.OnPhaseChange += BossPhaseHandler_OnPhaseChange;
    }

    private void OnDisable()
    {
        BossPhaseHandler.OnPhaseChange -= BossPhaseHandler_OnPhaseChange;
    }

    private void BlockInDirection()
    {
        Direction newBlockingDirection;

        if (randomizeDirection) newBlockingDirection = ChooseRandomDirection();
        else newBlockingDirection = ChooseFirstDirection();

        availableDirections.Remove(newBlockingDirection);
        currentBlockingDirections.Add(newBlockingDirection);

        OnBossBlock?.Invoke(this, new OnBossBlockEventArgs { direction = newBlockingDirection });
    }

    private Direction ChooseRandomDirection()
    {
        int randomIndex = UnityEngine.Random.Range(0, availableDirections.Count - 1);
        return availableDirections[randomIndex];
    }

    private Direction ChooseFirstDirection() => availableDirections[0];


    #region BossPhaseHandler Subsctiptions
    private void BossPhaseHandler_OnPhaseChange(object sender, BossPhaseHandler.OnPhaseChangeEventArgs e)
    {
        BlockInDirection();
    }
    #endregion
}
