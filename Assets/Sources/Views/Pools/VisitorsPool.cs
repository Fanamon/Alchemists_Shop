using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisitorsPool : MonoBehaviour
{
    [SerializeField] private int _count;

    [SerializeField] private Visitor _visitorPrefab;
    [SerializeField] private Transform _container;

    private List<Visitor> _visitors = new List<Visitor>();

    private void Awake()
    {
        SpawnPool();
    }

    public Visitor TryGetDisabledVisitor()
    {
        return _visitors.FirstOrDefault(visitor => visitor.gameObject.activeSelf == false);
    }

    private void SpawnPool()
    {
        Visitor spawnedVisitor;

        for (int i = 0; i < _count; ++i)
        {
            spawnedVisitor = Instantiate(_visitorPrefab, _container);
            spawnedVisitor.gameObject.SetActive(false);

            _visitors.Add(spawnedVisitor);
        }
    }
}
