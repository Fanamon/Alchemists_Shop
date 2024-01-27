using System.Collections;
using UnityEngine;

public class MathMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Transform _transform;

    private Coroutine _rotator = null;

    private void Awake()
    {
        _transform = transform;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        float scaledMoveSpeed = _speed * Time.deltaTime;

        Vector3 rotateDirection = Vector3.ProjectOnPlane(targetPosition - _transform.position, Vector3.up).normalized;

        if (_rotator != null)
        {
            StopCoroutine(_rotator);
        }

        _rotator = StartCoroutine(RotateTo(rotateDirection));
        _transform.position = Vector3.MoveTowards(_transform.position, targetPosition, scaledMoveSpeed);
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
