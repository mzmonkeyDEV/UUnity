public class ChaseState : IEnemyState
{
    private readonly EnemyAI _ai;
    private readonly EnemyMovementService _movement;

    public ChaseState(EnemyAI ai, EnemyMovementService movement)
    {
        _ai = ai;
        _movement = movement;
    }

    public void Enter() { }

    public void Execute()
    {
        if (_ai.Player != null)
        {
            _movement.MoveToDestination(_ai.Player.position);
        }
    }

    public void Exit() { }
}