using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;
    private readonly EnemyBehaviorProfile _profile;

    private readonly float _attackCooldown = 1.5f;
    private readonly float _attackRange = 2f;
    private readonly float _damage = 15f;
    private float _lastAttackTime;

    public MeleeAttackState(EnemyAI ai, EnemyMovementService movement, EnemyBehaviorProfile profile)
    {
        _ai = ai;
        _movement = movement;
        _profile = profile;
        _lastAttackTime = -_attackCooldown;
    }

    public void Enter()
    {
        _movement.Stop();
    }

    public void Execute()
    {
        if (_ai.Player == null) return;

        _movement.LookAt(_ai.Player);

        if (Time.time >= _lastAttackTime + _attackCooldown)
        {
            PerformMeleeAttack();
            _lastAttackTime = Time.time;
        }
    }

    public void Exit() { }

    private void PerformMeleeAttack()
    {
        Debug.Log("Melee attack!");

        // Check for hit
        Collider[] hits = Physics.OverlapSphere(_ai.transform.position, _attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                //var damageable = hit.GetComponent<IDamageable>();
                //damageable?.TakeDamage(_damage);
            }
        }
    }
}
