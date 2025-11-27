using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "AI/Enemy Configuration")]
public class EnemyConfig : ScriptableObject
{
    [Header("Movement")]
    public float walkPointRange = 10f;

    [Header("Combat")]
    public float sightRange = 15f;
    public float attackRange = 5f;
    public float timeBetweenAttacks = 2f;
    public GameObject projectile;
    public float projectileForce = 32f;
    public float projectileUpForce = 8f;

    [Header("Health")]
    public float maxHealth = 100f;

    [Header("Retreat Behavior")]
    [Range(0f, 1f)]
    public float retreatChance = 0.3f;
    public float retreatDistance = 8f;
    public float retreatCooldown = 5f;
}