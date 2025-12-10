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
    [Tooltip("Distance at which enemy will always retreat (personal space)")]
    public float tooCloseDistance = 3f;
    [Tooltip("Can interrupt attack state when player gets too close")]
    public bool canRetreatFromAttack = true;

    public enum EnemyType { Minion, Boss }

    [Header("Enemy Type")]
    public EnemyType enemyType = EnemyType.Minion;

    [Header("Boss Behaviour")]
 
    [Header("Skills")]
    public GameObject bigBoneProjectile;
    public GameObject rapidFireProjectile;
    public GameObject aoeBonePrefab;
    public GameObject minionPrefab;

    [Header("Skill Settings")]
    public float bigBoneForce = 20f;
    public float rapidFireForce = 12f;
    public float rapidFireRate = 0.1f;
    public int rapidFireCount = 8;
    public float aoeDelay = 1f;
    public float summonCount = 3;
    public float skillCooldown = 5f;

    [Header("Minion Settings")]
    public float meleeDamage = 10f;
    public float meleeRange = 2f;
    public float meleeCooldown = 1f;

    [Header("Boss Warp Skill")]
    public bool enableBossWarp = false;
    public Transform[] bossWalkPoints;
    public float bossWalkPointTolerance = 1.0f;
}