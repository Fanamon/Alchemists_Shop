using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MathMovement))]
[RequireComponent(typeof(Tiring))]
[RequireComponent(typeof(PotionDrinker))]
[RequireComponent(typeof(LeavingQueueAnimation))]
public class Visitor : MonoBehaviour
{
    private const float CompatibilityError = 0.01f;

    [SerializeField] private KeeperPlace _keeper;

    private Transform _transform;
    private MathMovement _movement;
    private Tiring _tiring;
    private PotionDrinker _drinker;
    private Coroutine _mover = null;
    private LeavingQueueAnimation _leavingQueueAnimation;

    public event UnityAction<Visitor> Left;

    private void Awake()
    {
        _transform = transform;
        _movement = GetComponent<MathMovement>();
        _tiring = GetComponent<Tiring>();
        _drinker = GetComponent<PotionDrinker>();
        _leavingQueueAnimation = GetComponent<LeavingQueueAnimation>();
    }

    private void OnEnable()
    {
        _tiring.Tired += OnTired;
        _drinker.Cured += OnFailed;
        _drinker.Failed += OnFailed;
    }

    private void OnDisable()
    {
        _tiring.Tired -= OnTired;
        _drinker.Cured -= OnFailed;
        _drinker.Failed -= OnFailed;
    }

    public void Reset(Transform startPlace)
    {
        _transform.position = startPlace.position;
        _transform.rotation = startPlace.rotation;
        _keeper.DropObject();

        gameObject.SetActive(true);
        _tiring.StartTiring();
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
        _tiring.StopTiring();
        _drinker.Drink(potion.GetComponent<Potion>());
    }

    public void LeaveQueue(Vector3 exitPosition)
    {
        StartCoroutine(Leave(exitPosition));
    }

    private void OnTired()
    {
        Left?.Invoke(this);
    }

    private void OnFailed()
    {
        Left?.Invoke(this);
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