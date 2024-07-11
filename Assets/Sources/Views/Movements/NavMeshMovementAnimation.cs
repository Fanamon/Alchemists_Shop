using UnityEngine;

[RequireComponent(typeof(NavMeshMovement))]
public class NavMeshMovementAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private NavMeshMovement _navMeshMovement;

    private void Awake()
    {
        _navMeshMovement = GetComponent<NavMeshMovement>();
    }

    private void OnEnable()
    {
        _navMeshMovement.MovementStarted += OnMovementStarted;
        _navMeshMovement.MovementFinished += OnMovementFinished;
    }

    private void OnDisable()
    {
        _navMeshMovement.MovementStarted -= OnMovementStarted;
        _navMeshMovement.MovementFinished -= OnMovementFinished;
    }

    private void OnMovementStarted()
    {
        if (_animator.GetFloat(AnimatorParameters.Speed) < AnimatorParameters.MoveSpeed)
        {
            _animator.SetFloat(AnimatorParameters.Speed, AnimatorParameters.MoveSpeed);
        }
    }

    private void OnMovementFinished()
    {
        if (_animator.GetFloat(AnimatorParameters.Speed) > AnimatorParameters.IdleSpeed)
        {
            _animator.SetFloat(AnimatorParameters.Speed, AnimatorParameters.IdleSpeed);
        }
    }
}