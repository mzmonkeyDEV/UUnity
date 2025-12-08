public class AttackState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;
    private readonly EnemyCombatService _combat;
    private readonly EnemyDetectionService _detection;

    public AttackState(EnemyAI ai, EnemyMovementService movement, EnemyCombatService combat, EnemyDetectionService detection)
    {
        _ai = ai;
        _movement = movement;
        _combat = combat;
        _detection = detection;
    }

    public void Enter() { }

    public void Execute()
    {
        if (_ai.Player == null) return;

        if (!_detection.HasLineOfSight(_ai.Player))
        {
            _ai.ChangeState(_ai.ChaseState);
            return;
        }

        _movement.Stop();
        _movement.LookAt(_ai.Player);
        _combat.Attack();
    }

    public void Exit() { }
}