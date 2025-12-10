using UnityEngine.AI;
using UnityEngine;

public class RetreatState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;
    private readonly EnemyConfig _config;
    private readonly Transform _transform;

    private float _retreatStartTime;
    private const float RETREAT_DURATION = 3f;
    private const float MIN_DISTANCE_FROM_PLAYER = 2f;

    public RetreatState(EnemyAI ai, EnemyMovementService movement, EnemyConfig config, Transform transform)
    {
        _ai = ai;
        _movement = movement;
        _config = config;
        _transform = transform;
    }

    public void Enter()
    {
        _retreatStartTime = Time.time;
    }

    public void Execute()
    {
        if (_ai.Player == null)
        {
            _ai.ChangeState(_ai.PatrolState);
            return;
        }

        // Calculate direction away from player
        Vector3 directionAway = (_transform.position - _ai.Player.position).normalized;

        // Calculate retreat position slightly ahead
        Vector3 retreatPosition = _transform.position + directionAway * 5f;

        // Try to find valid NavMesh position
        if (NavMesh.SamplePosition(retreatPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            _movement.MoveToDestination(hit.position);
        }
        else
        {
            // If can't find valid NavMesh, try moving perpendicular
            Vector3 perpendicular = Vector3.Cross(directionAway, Vector3.up).normalized;
            float direction = Random.value > 0.5f ? 1f : -1f;
            Vector3 sidePosition = _transform.position + perpendicular * direction * 5f;

            if (NavMesh.SamplePosition(sidePosition, out NavMeshHit sideHit, 10f, NavMesh.AllAreas))
            {
                _movement.MoveToDestination(sideHit.position);
            }
        }

        // Look at player while retreating
        _movement.LookAt(_ai.Player);

        // Check distance from player
        float distanceToPlayer = Vector3.Distance(_transform.position, _ai.Player.position);

        // Exit retreat if far enough or time expired
        //if (distanceToPlayer >= _config.retreatDistance || Time.time >= _retreatStartTime + RETREAT_DURATION)
            if (Time.time >= _retreatStartTime + RETREAT_DURATION)
            {
            _ai.ChangeState(_ai.ChaseState);
        }
    }

    public void Exit() { }
}
