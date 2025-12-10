using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEGroundExplosionSkill : ISkillExecutor
{
    private readonly GameObject _explosionPrefab;
    private readonly float _radius;
    private readonly float _damage;

    public AOEGroundExplosionSkill(GameObject explosionPrefab, float radius = 5f, float damage = 30f)
    {
        _explosionPrefab = explosionPrefab;
        _radius = radius;
        _damage = damage;
    }

    public void OnCastStart(Transform caster)
    {
        Debug.Log("Summoning ground explosion...");
    }

    public void Execute(Transform caster, Transform target)
    {
        Vector3 explosionPos = target.position;

        // Spawn visual effect
        if (_explosionPrefab != null)
        {
            Object.Instantiate(_explosionPrefab, explosionPos, Quaternion.identity);
        }

        // Deal damage to all in radius
        //Collider[] hits = Physics.OverlapSphere(explosionPos, _radius);
        //foreach (var hit in hits)
        //{
        //    var damageable = hit.GetComponent<IDamageable>();
        //    if (damageable != null && hit.CompareTag("Player"))
        //    {
        //        damageable.TakeDamage(_damage);
        //    }
        //}
    }

    public void OnCastEnd(Transform caster) { }
}
