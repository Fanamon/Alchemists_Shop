using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MathMovement))]
public class Visitor : MonoBehaviour
{
    private const float CompatibilityError = 0.01f;

    private const string LeaveLeft = "LeaveLeft";
    private const string LeaveRight = "LeaveRight";
    private const string Idle = "Idle";
    private const string LeavingLeft = "LeavingLeft";
    private const string LeavingRight = "LeavingRight";

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _model;
    [SerializeField] private KeeperPlace _keeper;

    private Transform _transform;
    private MathMovement _movement;
    private QueuePlace _place;
    private Vector3 _exitPosition;

    private string[] _leavingAnimations = { LeaveLeft, LeaveRight };
    private System.Random _random = new System.Random();

    public event UnityAction<Visitor> Leaved;

    private void Awake()
    {
        _transform = transform;
        _movement = GetComponent<MathMovement>();
    }

    public void InitializeExit(Vector3 exitPosition)
    {
        _exitPosition = exitPosition;
    }

    public void Reset(Transform startPlace)
    {
        _transform.position = startPlace.position;
        _transform.rotation = startPlace.rotation;
        _keeper.DropObject();

        gameObject.SetActive(true);
    }

    public void TakeNextPlace(QueuePlace place)
    {
        StartCoroutine(MoveToNextPlace(place));
    }

    public void TakePotion(Transform potion)
    {
        _keeper.Take(potion);

        LeaveQueue(_exitPosition);
    }

    public void LeaveQueue(Vector3 exitPosition)
    {
        StartCoroutine(Leave(exitPosition));
    }

    private IEnumerator MoveToNextPlace(QueuePlace place)
    {
        Vector3 placePosition = place.transform.position;

        while (CheckPositionsCompatibility(_transform.position, placePosition) == false)
        {
            _movement.MoveTo(placePosition);

            yield return null;
        }

        place.Take();

        if (_place != null)
        {
            _place.Leave();
        }

        _place = place;
    }

    private bool CheckPositionsCompatibility(Vector3 firstPosition, Vector3 secondPosition)
    {
        return Mathf.Abs(firstPosition.x - secondPosition.x) < CompatibilityError && 
            Mathf.Abs(firstPosition.z - secondPosition.z) < CompatibilityError;
    }

    private IEnumerator Leave(Vector3 exitPosition)
    {
        string triggerName = GetRandomTriggerName();
        bool isLeavingAnimationStarted = false;

        _animator.SetTrigger(triggerName);
        Leaved?.Invoke(this);
        _place.Leave();

        while (CheckPositionsCompatibility(_transform.position, exitPosition) == false)
        {
            if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == Idle && isLeavingAnimationStarted)
            {
                _movement.MoveTo(exitPosition);
            }
            else if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == LeavingLeft ||
                _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == LeavingRight)
            {
                isLeavingAnimationStarted = true;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }

    private string GetRandomTriggerName()
    {
        return _leavingAnimations[_random.Next(_leavingAnimations.Length)];
    }
}