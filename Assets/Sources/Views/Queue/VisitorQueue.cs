using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisitorQueue : MonoBehaviour
{
    [SerializeField] private Transform _queuePlacesKeeper;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private QueueGenerator _generator;

    private List<Visitor> _visitors = new List<Visitor>();
    private QueuePlace[] _queuePlaces;

    private void Awake()
    {
        _queuePlaces = _queuePlacesKeeper.GetComponentsInChildren<QueuePlace>();
    }

    private void OnEnable()
    {
        _generator.VisitorGenerated += OnVisitorGenerated;

        foreach (var place in _queuePlaces)
        {
            place.Emptied += OnPlaceEmptied;
        }
    }

    private void OnDisable()
    {
        _generator.VisitorGenerated -= OnVisitorGenerated;

        foreach (var visitor in _visitors)
        {
            visitor.Served -= OnVisitorServed;
        }

        foreach (var place in _queuePlaces)
        {
            place.Emptied -= OnPlaceEmptied;
        }
    }

    private void Start()
    {
        _generator.StartGenerating();
    }

    private void OnVisitorGenerated(Visitor visitor)
    {
        int leastEmptyPlaceIndex = GetLeastEmptyPlaceIndex();

        _visitors.Add(visitor);
        visitor.Served += OnVisitorServed;

        if (leastEmptyPlaceIndex == _queuePlaces.Count() - 1)
        {
            _generator.StopGenerating();
        }

        visitor.TakeNextPlace(_queuePlaces[leastEmptyPlaceIndex]);
    }

    private void OnVisitorServed(Visitor visitor)
    {
        _visitors.Remove(visitor);
        visitor.Served -= OnVisitorServed;
        visitor.LeaveQueue(_exitPoint.position);
    }

    private void OnPlaceEmptied(QueuePlace emptiedPlace)
    {
        if (_visitors.Count == 0)
        {
            return;
        }

        int placeIndex = System.Array.IndexOf(_queuePlaces, emptiedPlace);

        if (placeIndex == _queuePlaces.Length - 1)
        {
            _generator.StartGenerating();
        }
        else if (placeIndex < _visitors.Count)
        {
            _visitors[placeIndex].TakeNextPlace(_queuePlaces[placeIndex]);
        }
    }

    private int GetLeastEmptyPlaceIndex()
    {
        int leastEmptyPlaceIndex = System.Array.IndexOf(_queuePlaces, _queuePlaces.
            First(place => place.IsEmpty));

        if (leastEmptyPlaceIndex != 0)
        {
            int lastUnemptyPlaceIndex = System.Array.IndexOf(_queuePlaces, _queuePlaces.Reverse().
            First(place => place.IsEmpty == false));

            leastEmptyPlaceIndex = lastUnemptyPlaceIndex + 1;
        }

        return leastEmptyPlaceIndex;
    }
}