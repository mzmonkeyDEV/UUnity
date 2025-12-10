using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPatrolState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;
    private readonly EnemyBehaviorProfile _profile;

    private int _currentPointIndex;
    private bool _isMoving;

    public PositionPatrolState(EnemyAI ai, EnemyMovementService movement, EnemyBehaviorProfile profile)
    {
        _ai = ai;
        _movement = movement;
        _profile = profile;
        _currentPointIndex = 0;
    }

    public void Enter()
    {
        if (_profile.patrolPoints == null || _profile.patrolPoints.Length == 0)
        {
            _ai.ChangeState(_ai.ChaseState);
            return;
        }

        _isMoving = true;
    }

    public void Execute()
    {
        if (_profile.patrolPoints == null || _profile.patrolPoints.Length == 0) return;

        Transform targetPoint = _profile.patrolPoints[_currentPointIndex];
        _movement.MoveToDestination(targetPoint.position);

        if (_movement.IsAtDestination(targetPoint.position, 2f))
        {
            // Reached point, move to next
            _currentPointIndex = (_currentPointIndex + 1) % _profile.patrolPoints.Length;
        }
    }

    public void Exit() { }
}