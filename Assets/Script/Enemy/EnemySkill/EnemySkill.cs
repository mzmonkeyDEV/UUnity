using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySkill
{
    public string skillName;
    public float cooldown = 5f;
    public float castTime = 1f;
    public float range = 10f;
    public int priority = 1; // Higher priority skills used more often

    [HideInInspector] public float lastUsedTime = -999f;

    public bool CanUse()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public void MarkUsed()
    {
        lastUsedTime = Time.time;
    }
}