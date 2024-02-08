using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReagentPool : MonoBehaviour
{
    [SerializeField] private int _count;

    [SerializeField] private Transform _container;
    [SerializeField] private Reagent _reagentPrefab;

    private List<Reagent> _reagents = new List<Reagent>();

    private void Awake()
    {
        SpawnPool();
    }

    public Reagent TryGetDisabledReagent()
    {
        return _reagents.FirstOrDefault(reagent => reagent.gameObject.activeSelf == false);
    }

    private void SpawnPool()
    {
        Reagent spawnedReagent;

        for (int i = 0; i < _count; i++)
        {
            spawnedReagent = Instantiate(_reagentPrefab, _container);
            spawnedReagent.gameObject.SetActive(false);

            _reagents.Add(spawnedReagent);
        }
    }
}
