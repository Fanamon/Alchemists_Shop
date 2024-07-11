using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshMovement))]
[RequireComponent(typeof(Tiring))]
[RequireComponent(typeof(PotionDrinker))]
[RequireComponent(typeof(VisitorModelsAnimationsView))]
public class Visitor : MonoBehaviour
{
    private const float CompatibilityError = 0.01f;

    [SerializeField] private KeeperPlace _keeper;

    private Transform _transform;
    private NavMeshMovement _movement;
    private Tiring _tiring;
    private PotionDrinker _drinker;
    private Coroutine _mover = null;
    private VisitorModelsAnimationsView _visitorModelsAnimationsView;

    public event UnityAction<Visitor> Left;

    private void Awake()
    {
        _transform = transform;
        _movement = GetComponent<NavMeshMovement>();
        _tiring = GetComponent<Tiring>();
        _drinker = GetComponent<PotionDrinker>();
        _visitorModelsAnimationsView = GetComponent<VisitorModelsAnimationsView>();
    }

    private void OnEnable()
    {
        _tiring.Tired += OnTired;
        _visitorModelsAnimationsView.DrinkingFinished += OnDrinkingFinished;
    }

    private void OnDisable()
    {
        _tiring.Tired -= OnTired;
        _visitorModelsAnimationsView.DrinkingFinished -= OnDrinkingFinished;
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
        _visitorModelsAnimationsView.Drink();
        _drinker.Drink(potion.GetComponent<Potion>(), _visitorModelsAnimationsView.DrinkingTime);
    }

    public void LeaveQueue(Vector3 exitPosition)
    {
        if (_mover != null)
        {
            StopCoroutine(_mover);
        }

        _mover = StartCoroutine(MoveToNextPlace(exitPosition));
    }

    private void OnTired()
    {
        Left?.Invoke(this);
    }

    private void OnDrinkingFinished()
    {
        Left?.Invoke(this);
    }

    private IEnumerator MoveToNextPlace(Vector3 targetPlace)
    {
        _visitorModelsAnimationsView.Walk();
        _movement.MoveTo(targetPlace);

        while (CheckPositionsCompatibility(_transform.position, targetPlace) == false)
        {
            yield return null;
        }

        _visitorModelsAnimationsView.Idle();
        _mover = null;
    }

    private bool CheckPositionsCompatibility(Vector3 firstPosition, Vector3 secondPosition)
    {
        return Mathf.Abs(firstPosition.x - secondPosition.x) < CompatibilityError && 
            Mathf.Abs(firstPosition.z - secondPosition.z) < CompatibilityError;
    }
}