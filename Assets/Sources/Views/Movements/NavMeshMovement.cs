using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UniRx;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : MonoBehaviour
{
    private const float MinMoveSensitivity = 0.1f;

    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private CompositeDisposable _disposable;

    public event UnityAction MovementStarted;
    public event UnityAction MovementFinished;

    private void Awake()
    {
        _transform = transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _disposable = new CompositeDisposable();
    }

    private void OnDisable()
    {
        _disposable?.Dispose();
        _disposable?.Clear();
    }

    public void MoveTo(Vector2 direction)
    {
        if (direction.sqrMagnitude < MinMoveSensitivity)
        {
            _disposable = new CompositeDisposable();
            Observable.EveryLateUpdate().Subscribe(_ => UpdateNavMeshDestination()).AddTo(_disposable);

            MovementFinished?.Invoke();
            return;
        }

        _disposable?.Dispose();
        _disposable?.Clear();

        Vector3 endMovementPoint = new Vector3(_transform.position.x + direction.x, _transform.position.y, 
            _transform.position.z + direction.y);

        _navMeshAgent.SetDestination(endMovementPoint);
        MovementStarted?.Invoke();
    }

    public void MoveTo(Vector3 targetPoint)
    {
        _navMeshAgent.SetDestination(targetPoint);
    }

    private void UpdateNavMeshDestination()
    {
        _navMeshAgent.SetDestination(_transform.position);
    }
}
