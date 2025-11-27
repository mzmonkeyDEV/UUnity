using UnityEngine;

public class EnemyCombatService
{
    private readonly Transform _transform;
    private readonly EnemyConfig _config;
    private float _lastAttackTime;

    public EnemyCombatService(Transform transform, EnemyConfig config)
    {
        _transform = transform;
        _config = config;
        _lastAttackTime = -_config.timeBetweenAttacks;
    }

    public bool CanAttack()
    {
        return Time.time >= _lastAttackTime + _config.timeBetweenAttacks;
    }

    public void Attack()
    {
        if (!CanAttack()) return;

        GameObject projectileObj = Object.Instantiate(_config.projectile, _transform.position, Quaternion.identity);
        Rigidbody rb = projectileObj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(_transform.forward * _config.projectileForce, ForceMode.Impulse);
            rb.AddForce(_transform.up * _config.projectileUpForce, ForceMode.Impulse);
        }

        _lastAttackTime = Time.time;
    }
}
