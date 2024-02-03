using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MathMovement))]
[RequireComponent(typeof(LeavingQueueAnimation))]
public class Visitor : MonoBehaviour
{
    private const float CompatibilityError = 0.01f;

    [SerializeField] private KeeperPlace _keeper;

    private Transform _transform;
    private MathMovement _movement;
    private Coroutine _mover = null;
    private LeavingQueueAnimation _leavingQueueAnimation;

    public event UnityAction<Visitor> Left;

    private void Awake()
    {
        _transform = transform;
        _movement = GetComponent<MathMovement>();
        _leavingQueueAnimation = GetComponent<LeavingQueueAnimation>();
    }

    public void Reset(Transform startPlace)
    {
        _transform.position = startPlace.position;
        _transform.rotation = startPlace.rotation;
        _keeper.DropObject();

        gameObject.SetActive(true);
    }

    public void TakeNextPlace(Vector3 targetPlace)
    {
        if (_mover != null)
        {
            StopCoroutine(_mover);
        }

        _mover = StartCoroutine(MoveToNextPlace(targetPlace));
    }

    public void TakePotion(Transform potion)
    {
        _keeper.Take(potion);

        Left?.Invoke(this);
    }

    public void LeaveQueue(Vector3 exitPosition)
    {
        StartCoroutine(Leave(exitPosition));
    }

    private IEnumerator MoveToNextPlace(Vector3 targetPlace)
    {
        while (CheckPositionsCompatibility(_transform.position, targetPlace) == false)
        {
            _movement.MoveTo(targetPlace);

            yield return null;
        }

        _mover = null;
    }

    private bool CheckPositionsCompatibility(Vector3 firstPosition, Vector3 secondPosition)
    {
        return Mathf.Abs(firstPosition.x - secondPosition.x) < CompatibilityError && 
            Mathf.Abs(firstPosition.z - secondPosition.z) < CompatibilityError;
    }

    private IEnumerator Leave(Vector3 exitPosition)
    {
        _leavingQueueAnimation.StartAnimation();

        while (CheckPositionsCompatibility(_transform.position, exitPosition) == false)
        {
            if (_leavingQueueAnimation.IsStarted == false)
            {
                _movement.MoveTo(exitPosition);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}