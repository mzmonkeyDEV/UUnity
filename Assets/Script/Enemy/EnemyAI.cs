using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : Character
{
    public int SkillUseCount { get; private set; }

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
        _combat = new EnemyCombatService(transform, config,this);
    }

    private void InitializeStates()
    {
        PatrolState = new PatrolState(this, _movement, config);
        ChaseState = new ChaseState(this, _movement);
        AttackState = new AttackState(this, _movement, _combat, _detection,config);
        RetreatState = new RetreatState(this, _movement, config, transform);
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
        bool playerInAttack = _detection.IsPlayerInAttackRange(player);

        // Check for retreat condition first
        if (playerInSight && ShouldRetreat())
        {
            ChangeState(RetreatState);
            _lastRetreatTime = Time.time;
            return;
        }

        // Normal state transitions
        if (!playerInSight && !playerInAttack && _currentState != PatrolState)
        {
            ChangeState(PatrolState);
        }
        else if (playerInSight && !playerInAttack && _currentState != ChaseState && _currentState != RetreatState && !config.enableBossWarp)
        {
            ChangeState(ChaseState);
        }
        else if (playerInAttack && playerInSight && _currentState != AttackState)
        {
            ChangeState(AttackState);
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
    public void RegisterSkillUsed()
    {
        SkillUseCount++;

        if (ShouldWarpAfterSkill())
        {
            animator.SetTrigger("Warp");
            
            SkillUseCount = 0; // reset counter
        }
    }

    private bool ShouldWarpAfterSkill()
    {
        if (!config.enableBossWarp) return false;
        if (config.bossWalkPoints == null || config.bossWalkPoints.Length == 0) return false;

        if (SkillUseCount >= 3) return true;             // guaranteed
        return Random.value < 0.2f;                     
    }

    private int _bossWarpIndex = 0;

    private void WarpToNextBossPoint()
    {
        _bossWarpIndex = (_bossWarpIndex + 1) % config.bossWalkPoints.Length;

        Transform point = config.bossWalkPoints[_bossWarpIndex];

        agent.Warp(point.position);       // instant teleport
        transform.rotation = point.rotation;

        Debug.Log("Boss warped to point " + _bossWarpIndex);
    }
    public void AnimEvent_BigBone()
    {
        _combat.Event_SkillBigBone_Release();
    }

    public void AnimEvent_RapidFire()
    {
        StartCoroutine(_combat.RapidFireCoroutine());
    }

    public void AnimEvent_AOE()
    {
        _combat.Event_SkillAOE_Release();
    }

    public void AnimEvent_Summon()
    {
        _combat.Event_SkillSummon_Spawn();
    }

    public void AnimEvent_Melee()
    {
        if (config.enemyType == EnemyConfig.EnemyType.Minion)
            _combat.MeleeDealDamage();
    }

}