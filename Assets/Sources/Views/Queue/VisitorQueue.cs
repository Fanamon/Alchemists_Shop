using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisitorQueue : MonoBehaviour
{
    [SerializeField] private Transform _queuePlacesKeeper;
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

        foreach (var visitor in _visitors)
        {
            visitor.Leaved += OnVisitorLeaved;
        }

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
            visitor.Leaved -= OnVisitorLeaved;
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
        int firstEmptyPlace = System.Array.IndexOf(_queuePlaces, _queuePlaces.
            First(place => place.IsEmpty));

        _visitors.Append(visitor);

        if (firstEmptyPlace == _queuePlaces.Count() - 1)
        {
            _generator.StopGenerating();
        }

        visitor.TakeNextPlace(_queuePlaces[firstEmptyPlace]);
    }

    private void OnVisitorLeaved(Visitor visitor)
    {
        _visitors.Remove(visitor);
    }

    private void OnPlaceEmptied(QueuePlace emptiedPlace)
    {
        if (_generator.IsGenerating)
        {
            return;
        }

        int placeIndex = System.Array.IndexOf(_queuePlaces, emptiedPlace);

        if (placeIndex == _queuePlaces.Length - 1)
        {
            _generator.StartGenerating();
        }
        else if (_queuePlaces[placeIndex + 1].IsEmpty == false)
        {
            _visitors[placeIndex].TakeNextPlace(_queuePlaces[placeIndex]);
        }
    }
}