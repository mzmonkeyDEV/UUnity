using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : Character
{
    [Header("Behavior")]
    [SerializeField] private EnemyBehaviorProfile behaviorProfile;
    private Dictionary<string, ISkillExecutor> _skillExecutors;

    [Header("References")]
    [SerializeField] private EnemyConfig config;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Animator animator;

    // Services
    private EnemyDetectionService _detection;
    private EnemyMovementService _movement;
    private EnemyCombatService _combat;

    // States
    private IEnemyState _currentState;
    public PatrolState PatrolState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public RetreatState RetreatState { get; private set; }
    public SkillState SkillState { get; private set; }
    public MeleeAttackState MeleeAttackState { get; private set; }
    public PositionPatrolState PositionPatrolState { get; private set; }

    // Properties
    public Transform Player => player;
    private float _health;
    private float _lastRetreatTime;

    private void Awake()
    {
        InitializeReferences();
        InitializeServices();
        InitializeStates();

        _health = config.maxHealth;
        _lastRetreatTime = -config.retreatCooldown;
        _currentState = PatrolState;
        _currentState.Enter();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void InitializeReferences()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    private void InitializeServices()
    {
        _detection = new EnemyDetectionService(transform, config, whatIsPlayer, whatIsWall);
        _movement = new EnemyMovementService(agent, transform, whatIsGround);
        _combat = new EnemyCombatService(transform, config);
    }

    private void InitializeStates()
    {
        // Initialize based on behavior profile
        if (behaviorProfile.usePositionPoints)
        {
            PositionPatrolState = new PositionPatrolState(this, _movement, behaviorProfile);
        }
        else if (behaviorProfile.usePatrol)
        {
            PatrolState = new PatrolState(this, _movement, config);
        }

        if (behaviorProfile.useChase)
        {
            ChaseState = new ChaseState(this, _movement);
        }

        if (behaviorProfile.useRetreat)
        {
            RetreatState = new RetreatState(this, _movement, config, transform);
        }

        if (behaviorProfile.useMeleeAttack)
        {
            MeleeAttackState = new MeleeAttackState(this, _movement, behaviorProfile);
        }

        if (behaviorProfile.useRangedAttack)
        {
            AttackState = new AttackState(this, _movement, _combat, _detection);
        }

        if (behaviorProfile.useSkillSystem)
        {
            SkillState = new SkillState(this, _movement, behaviorProfile);
            InitializeSkillExecutors();
        }

        // Set initial state based on profile
        if (behaviorProfile.usePositionPoints)
        {
            _currentState = PositionPatrolState;
        }
        else if (behaviorProfile.usePatrol)
        {
            _currentState = PatrolState;
        }
        else
        {
            _currentState = ChaseState;
        }

        _currentState?.Enter();
    }

    private void Update()
    {
        if (player == null) return;
        
        _currentState.Execute();
        EvaluateStateTransitions();
        animator.SetFloat("Speed",agent.velocity.magnitude);
    }

    private void EvaluateStateTransitions()
    {
        bool playerInSight = _detection.IsPlayerInSightRange(player);
        bool playerInRange = Vector3.Distance(transform.position, player.position) <= behaviorProfile.engageDistance;

        // Boss with skill system
        if (behaviorProfile.useSkillSystem)
        {
            if (playerInSight && SkillState.IsComplete())
            {
                // Randomly decide to use skill or move
                if (Random.value < 0.7f && behaviorProfile.availableSkills.Any(s => s.CanUse()))
                {
                    ChangeState(SkillState);
                }
                else if (behaviorProfile.usePositionPoints)
                {
                    ChangeState(PositionPatrolState);
                }
                else
                {
                    ChangeState(ChaseState);
                }
            }
        }
        // Melee enemy
        else if (behaviorProfile.useMeleeAttack)
        {
            if (playerInRange && _currentState != MeleeAttackState)
            {
                ChangeState(MeleeAttackState);
            }
            else if (!playerInRange && _currentState != ChaseState)
            {
                ChangeState(ChaseState);
            }
        }
        // Ranged enemy (use original logic)
        else
        {
            // Keep your original transition logic here
            if (behaviorProfile.useRetreat && playerInSight && ShouldRetreat())
            {
                ChangeState(RetreatState);
                _lastRetreatTime = Time.time;
                return;
            }

            if (!playerInSight && _currentState != PatrolState)
            {
                ChangeState(PatrolState);
            }
            else if (playerInSight && !playerInRange && _currentState != ChaseState)
            {
                ChangeState(ChaseState);
            }
            else if (playerInRange && playerInSight && _currentState != AttackState)
            {
                ChangeState(AttackState);
            }
        }
    }

    private bool ShouldRetreat()
    {
        if (_currentState == RetreatState) return false;
        if (Time.time < _lastRetreatTime + config.retreatCooldown) return false;

        return Random.value < config.retreatChance;
    }

    public void ChangeState(IEnemyState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    //public void TakeDamage(float damage)
    //{
    //    _health -= damage;
    //    if (_health <= 0)
    //    {
    //        Die();
    //    }
    //}

    //private void Die()
    //{
    //    Destroy(gameObject, 0.5f);
    //}

    private void OnDrawGizmosSelected()
    {
        if (config == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, config.attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, config.sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, config.retreatDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, config.tooCloseDistance);
    }

    private void InitializeSkillExecutors()
    {
        _skillExecutors = new Dictionary<string, ISkillExecutor>
        {
            { "SpinningBoneShot", new SpinningBoneShotSkill(config.projectile, 8, 20f) },
            { "TwoWayRapidShot", new TwoWayRapidShotSkill(config.projectile, 5, 25f) },
            { "AOEExplosion", new AOEGroundExplosionSkill(config.projectile, 5f, 30f) },
            { "SummonMinions", new SummonMinionsSkill(config.projectile, 3, 3f) }
        };
    }

    public ISkillExecutor GetSkillExecutor(string skillName)
    {
        return _skillExecutors.ContainsKey(skillName) ? _skillExecutors[skillName] : null;
    }


}