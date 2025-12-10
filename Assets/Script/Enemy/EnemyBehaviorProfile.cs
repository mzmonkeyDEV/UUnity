using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBehaviorProfile", menuName = "AI/Enemy Behavior Profile")]
public class EnemyBehaviorProfile : ScriptableObject
{
    [Header("Behavior Type")]
    public EnemyType enemyType;

    [Header("State Configuration")]
    public bool usePatrol = true;
    public bool useChase = true;
    public bool useRetreat = false;
    public bool useMeleeAttack = false;
    public bool useRangedAttack = false;
    public bool useSkillSystem = false;

    [Header("Movement Behavior")]
    public bool usePositionPoints = false; // For bosses that move to specific points
    public Transform[] patrolPoints; // Specific points to move between

    [Header("Combat Behavior")]
    public float engageDistance = 2f; // Distance to start attacking
    public bool shouldFacePlayer = true;

    [Header("Boss Specific")]
    public bool isBoss = false;
    public List<EnemySkill> availableSkills = new List<EnemySkill>();
}