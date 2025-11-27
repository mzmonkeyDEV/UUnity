using UnityEngine.AI;
using UnityEngine;
public class PatrolState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;
    private readonly EnemyConfig _config;
    private Vector3 _walkPoint;
    private bool _walkPointSet;

    public PatrolState(EnemyAI ai, EnemyMovementService movement, EnemyConfig config)
    {
        _ai = ai;
        _movement = movement;
        _config = config;
    }

    public void Enter()
    {
        _walkPointSet = false;
    }

    public void Execute()
    {
        if (!_walkPointSet)
        {
            if (_movement.FindRandomPointOnNavMesh(_config.walkPointRange, out _walkPoint))
            {
                _walkPointSet = true;
            }
        }

        if (_walkPointSet)
        {
            _movement.MoveToDestination(_walkPoint);

            if (_movement.IsAtDestination(_walkPoint))
            {
                _walkPointSet = false;
            }
        }
    }

    public void Exit() { }
}