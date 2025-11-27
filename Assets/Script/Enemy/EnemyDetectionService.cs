using UnityEngine;

public class EnemyDetectionService
{
    private readonly Transform _transform;
    private readonly EnemyConfig _config;
    private readonly LayerMask _playerLayer;
    private readonly LayerMask _wallLayer;

    public EnemyDetectionService(Transform transform, EnemyConfig config, LayerMask playerLayer, LayerMask wallLayer)
    {
        _transform = transform;
        _config = config;
        _playerLayer = playerLayer;
        _wallLayer = wallLayer;
    }

    public bool IsPlayerInSightRange(Transform player)
    {
        return Physics.CheckSphere(_transform.position, _config.sightRange, _playerLayer);
    }

    public bool IsPlayerInAttackRange(Transform player)
    {
        return Physics.CheckSphere(_transform.position, _config.attackRange, _playerLayer);
    }

    public bool HasLineOfSight(Transform player)
    {
        Vector3 direction = player.position - _transform.position;
        return !Physics.Raycast(_transform.position, direction, _config.attackRange, _wallLayer);
    }
}