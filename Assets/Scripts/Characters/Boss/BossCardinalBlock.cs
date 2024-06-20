using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossCardinalBlock : MonoBehaviour
{
    [Header("Directions")]
    [SerializeField] private List<Direction> availableDirections;
    [SerializeField] private bool randomizeDirection;

    [Header("Direction Blocks")]
    [SerializeField] private List<DirectionBlock> directionBlocks;
    [Space]
    [SerializeField] private List<Direction> currentBlockingDirections;

    [Header("Block Since Start")]
    [SerializeField] private bool blockSinceStart;

    public enum Direction {Front, Back, Left, Right}

    public static event EventHandler<OnBossBlockEventArgs> OnBossBlock;

    [Serializable]
    public class DirectionBlock
    {
        public Direction direction;
        public Transform block;
    }

    public class OnBossBlockEventArgs : EventArgs
    {
        public Direction direction;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossActiveEnd += BossStateHandler_OnBossActiveEnd;
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossActiveEnd -= BossStateHandler_OnBossActiveEnd;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
    }

    private void BlockInDirection()
    {
        Direction newBlockingDirection;

        if (randomizeDirection) newBlockingDirection = ChooseRandomDirection();
        else newBlockingDirection = ChooseFirstDirection();

        availableDirections.Remove(newBlockingDirection);
        currentBlockingDirections.Add(newBlockingDirection);

        EnableDirectionBlock(newBlockingDirection);

        OnBossBlock?.Invoke(this, new OnBossBlockEventArgs { direction = newBlockingDirection });
    }

    private Direction ChooseRandomDirection()
    {
        int randomIndex = UnityEngine.Random.Range(0, availableDirections.Count - 1);
        return availableDirections[randomIndex];
    }

    private Direction ChooseFirstDirection() => availableDirections[0];

    private void EnableDirectionBlock(Direction direction)
    {
        foreach(DirectionBlock directionBlock in directionBlocks)
        {
            if (directionBlock.direction == direction)
            {
                directionBlock.block.gameObject.SetActive(true);
                return;
            }
        }
    }

    private void DisableAllDirectionBlocks()
    {
        foreach (DirectionBlock directionBlock in directionBlocks)
        {
            directionBlock.block.gameObject.SetActive(false);
        }
    }

    #region BossPhaseHandler Subsctiptions
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e)
    {
        if (!blockSinceStart) return;
        BlockInDirection();
    }
    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, EventArgs e)
    {
        BlockInDirection();
    }
    #endregion

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        DisableAllDirectionBlocks();
    }
    #endregion
}
