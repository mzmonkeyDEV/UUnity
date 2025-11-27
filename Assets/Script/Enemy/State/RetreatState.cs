using UnityEngine.AI;
using UnityEngine;
public class RetreatState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;
    private readonly EnemyConfig _config;
    private Vector3 _retreatPoint;
    private bool _retreatPointSet;

    public RetreatState(EnemyAI ai, EnemyMovementService movement, EnemyConfig config)
    {
        _ai = ai;
        _movement = movement;
        _config = config;
    }

    public void Enter()
    {
        _retreatPointSet = false;
        FindRetreatPoint();
    }

    public void Execute()
    {
        if (!_retreatPointSet) return;

        _movement.MoveToDestination(_retreatPoint);

        if (_movement.IsAtDestination(_retreatPoint, 2f))
        {
            _ai.ChangeState(_ai.PatrolState);
        }
    }

    public void Exit() { }

    private void FindRetreatPoint()
    {
        if (_ai.Player == null) return;

        Vector3 directionAwayFromPlayer = (_ai.transform.position - _ai.Player.position).normalized;
        Vector3 retreatPosition = _ai.transform.position + directionAwayFromPlayer * _config.retreatDistance;

        if (_movement.FindRandomPointOnNavMesh(_config.retreatDistance, out Vector3 validPoint))
        {
            _retreatPoint = validPoint;
            _retreatPointSet = true;
        }
        else
        {
            _retreatPoint = retreatPosition;
            _retreatPointSet = true;
        }
    }
}