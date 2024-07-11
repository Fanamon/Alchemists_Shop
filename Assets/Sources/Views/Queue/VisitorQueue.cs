using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisitorQueue : MonoBehaviour
{
    [SerializeField] private Transform _queuePlacesKeeper;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private QueueGenerator _generator;

    private bool _isRotateQueue = false;

    private Dictionary<Transform, Visitor> _queue = new Dictionary<Transform, Visitor>();

    private void Awake()
    {
        QueuePlace[] queuePlaces = _queuePlacesKeeper.GetComponentsInChildren<QueuePlace>();

        foreach (QueuePlace place in queuePlaces)
        {
            _queue.Add(place.transform, null);
        }
    }

    private void OnEnable()
    {
        _generator.VisitorGenerated += OnVisitorGenerated;
    }

    private void OnDisable()
    {
        _generator.VisitorGenerated -= OnVisitorGenerated;

        foreach (var visitor in _queue.Values.Where(visitor => visitor != null))
        {
            visitor.Left -= OnVisitorLeft;
        }
    }

    public void StartRotateQueue()
    {
        _isRotateQueue = true;
    }

    public void StopRotateQueue()
    {
        _isRotateQueue = false;
    }

    private void OnVisitorGenerated(Visitor visitor)
    {
        Transform emptyPlace = _queue.FirstOrDefault(place => place.Value == null).Key;

        if (emptyPlace == null)
        {
            return;
        }

        int firstEmptyPlaceIndex = _queue.Keys.ToList().IndexOf(emptyPlace);

        _queue[emptyPlace] = visitor;
        visitor.Left += OnVisitorLeft;
        visitor.TakeNextPlace(emptyPlace.position);

        if (firstEmptyPlaceIndex == _queue.Count() - 1)
        {
            _generator.StopGenerating();
        }
    }

    private void OnVisitorLeft(Visitor leftVisitor)
    {
        Transform leftPlace = _queue.First(place => place.Value == leftVisitor).Key;

        _queue[leftPlace] = null;
        leftVisitor.Left -= OnVisitorLeft;
        leftVisitor.LeaveQueue(_exitPoint.position);

        CarryOutQueueRotation(leftPlace);
    }

    private void CarryOutQueueRotation(Transform leftplace)
    {
        int leftPlaceIndex = _queue.Keys.ToList().IndexOf(leftplace);

        if (_generator.IsGenerating == false && _isRotateQueue)
        {
            _generator.StartGenerating();
        }

        if (leftPlaceIndex != _queue.Count() - 1)
        {
            TakeVisitorsNextPlacesFrom(leftPlaceIndex);
        }
    }

    private void TakeVisitorsNextPlacesFrom(int leftPlaceIndex)
    {
        List<Transform> placesToCarry = _queue.Keys.Skip(leftPlaceIndex).ToList();

        for (int i = 0; i < placesToCarry.Count - 1; i++)
        {
            Visitor visitorToMove = _queue[placesToCarry[i + 1]];

            if (visitorToMove == null)
            {
                break;
            }

            _queue[placesToCarry[i]] = visitorToMove;
            _queue[placesToCarry[i + 1]] = null;

            visitorToMove.TakeNextPlace(placesToCarry[i].position);
        }
    }
}