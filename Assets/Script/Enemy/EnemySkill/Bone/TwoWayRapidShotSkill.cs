using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayRapidShotSkill : ISkillExecutor
{
    private readonly GameObject _projectilePrefab;
    private readonly int _volleyCount;
    private readonly float _projectileSpeed;

    public TwoWayRapidShotSkill(GameObject projectilePrefab, int volleys = 5, float speed = 25f)
    {
        _projectilePrefab = projectilePrefab;
        _volleyCount = volleys;
        _projectileSpeed = speed;
    }

    public void OnCastStart(Transform caster)
    {
        Debug.Log("Casting Two-Way Rapid Shot...");
    }

    public void Execute(Transform caster, Transform target)
    {
        Vector3 toTarget = (target.position - caster.position).normalized;
        Vector3 perpendicular = Vector3.Cross(toTarget, Vector3.up).normalized;

        // Fire projectiles in two rotating directions
        for (int i = 0; i < _volleyCount; i++)
        {
            float rotationAngle = i * 30f;

            // First direction
            Vector3 dir1 = Quaternion.Euler(0, rotationAngle, 0) * toTarget;
            FireProjectile(caster.position, dir1);

            // Opposite direction
            Vector3 dir2 = Quaternion.Euler(0, -rotationAngle, 0) * toTarget;
            FireProjectile(caster.position, dir2);
        }
    }

    private void FireProjectile(Vector3 position, Vector3 direction)
    {
        GameObject projectile = Object.Instantiate(_projectilePrefab, position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * _projectileSpeed;
        }
    }

    public void OnCastEnd(Transform caster) { }
}
