using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementService
{
    private readonly NavMeshAgent _agent;
    private readonly Transform _transform;
    private readonly LayerMask _groundLayer;

    public EnemyMovementService(NavMeshAgent agent, Transform transform, LayerMask groundLayer)
    {
        _agent = agent;
        _transform = transform;
        _groundLayer = groundLayer;
    }

    public void MoveToDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }

    public void Stop()
    {
        _agent.SetDestination(_transform.position);
    }

    public bool IsAtDestination(Vector3 destination, float threshold = 1f)
    {
        return Vector3.Distance(_transform.position, destination) < threshold;
    }

    public void LookAt(Transform target)
    {
        _transform.LookAt(target);
    }

    public bool FindRandomPointOnNavMesh(float range, out Vector3 point)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = _transform.position + new Vector3(
                Random.Range(-range, range),
                0f,
                Random.Range(-range, range)
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, range, NavMesh.AllAreas))
            {
                if (Physics.Raycast(hit.position, Vector3.down, 2f, _groundLayer))
                {
                    point = hit.position;
                    return true;
                }
            }
        }

        point = _transform.position;
        return false;
    }
}