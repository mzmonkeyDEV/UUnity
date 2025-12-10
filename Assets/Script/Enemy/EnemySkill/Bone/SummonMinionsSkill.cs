using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinionsSkill : ISkillExecutor
{
    private readonly GameObject _minionPrefab;
    private readonly int _minionCount;
    private readonly float _spawnRadius;

    public SummonMinionsSkill(GameObject minionPrefab, int count = 3, float radius = 3f)
    {
        _minionPrefab = minionPrefab;
        _minionCount = count;
        _spawnRadius = radius;
    }

    public void OnCastStart(Transform caster)
    {
        Debug.Log("Summoning minions...");
    }

    public void Execute(Transform caster, Transform target)
    {
        for (int i = 0; i < _minionCount; i++)
        {
            float angle = i * (360f / _minionCount);
            Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * _spawnRadius;
            Vector3 spawnPos = caster.position + offset;

            Object.Instantiate(_minionPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void OnCastEnd(Transform caster)
    {
        Debug.Log("Minions summoned!");
    }
}
