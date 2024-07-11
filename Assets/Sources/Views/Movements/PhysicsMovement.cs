using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsMovement : MonoBehaviour
{
    private const float MinMoveSensitivity = 0.1f;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Transform _transform;
    private Rigidbody _rigidbody;

    private Coroutine _rotator = null;

    public event UnityAction MovementStarted;
    public event UnityAction MovementFinished;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveTo(Vector2 direction)
    {
        if (direction.sqrMagnitude < MinMoveSensitivity)
        {
            MovementFinished?.Invoke();
            return;
        }

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        Vector3 rotateDirection = Vector3.ProjectOnPlane(moveDirection, Vector3.up).normalized;

        if (_rotator != null)
        {
            StopCoroutine(_rotator);
        }

        _rotator = StartCoroutine(RotateTo(rotateDirection));
        _rigidbody.velocity = moveDirection.normalized * _speed;
        MovementStarted?.Invoke();
    }

    private IEnumerator RotateTo(Vector3 direction)
    {
        float scaledRotationSpeed = _rotationSpeed + Time.deltaTime;

        while (_transform.forward != direction)
        {
            _transform.forward = Vector3.Slerp(_transform.forward, direction, scaledRotationSpeed);

            yield return null;
        }

        _rotator = null;
    }
}
