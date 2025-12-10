using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBoneShotSkill : ISkillExecutor
{
    private readonly GameObject _projectilePrefab;
    private readonly int _projectileCount;
    private readonly float _projectileSpeed;

    public SpinningBoneShotSkill(GameObject projectilePrefab, int count = 8, float speed = 20f)
    {
        _projectilePrefab = projectilePrefab;
        _projectileCount = count;
        _projectileSpeed = speed;
    }

    public void OnCastStart(Transform caster)
    {
        Debug.Log("Casting Spinning Bone Shot...");
        // Play cast animation
    }

    public void Execute(Transform caster, Transform target)
    {
        float angleStep = 360f / _projectileCount;

        for (int i = 0; i < _projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            GameObject projectile = Object.Instantiate(_projectilePrefab, caster.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = direction * _projectileSpeed;
            }
        }
    }

    public void OnCastEnd(Transform caster)
    {
        Debug.Log("Spinning Bone Shot complete!");
    }
}

