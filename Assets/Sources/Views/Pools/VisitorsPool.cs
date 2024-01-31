using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisitorsPool : MonoBehaviour
{
    [SerializeField] private int _eachVisitorTypeCount;

    [SerializeField] private List<Visitor> _visitorPrefabs;
    [SerializeField] private Transform _container;

    private Dictionary<DiseaseType, List<Visitor>> _visitors = new Dictionary<DiseaseType, List<Visitor>>();

    private void Awake()
    {
        SpawnPool();
    }

    public Visitor TryGetDisabledVisitor(DiseaseType disease)
    {
        return _visitors[disease].FirstOrDefault(visitor => visitor.gameObject.activeSelf == false);
    }

    private void SpawnPool()
    {
        Visitor spawnedVisitor;

        for (int i = 0; i < _visitorPrefabs.Count; i++)
        {
            DiseaseType diseaseType = _visitorPrefabs[i].GetComponent<Disease>().Type;
            _visitors[diseaseType] = new List<Visitor>();

            for (int j = 0; j < _eachVisitorTypeCount; j++)
            {
                spawnedVisitor = Instantiate(_visitorPrefabs[i], _container);
                spawnedVisitor.gameObject.SetActive(false);

                _visitors[diseaseType].Add(spawnedVisitor);
            }
        }
    }
}
