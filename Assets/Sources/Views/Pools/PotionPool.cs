using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionPool : MonoBehaviour
{
    [SerializeField] private int _eachPotionTypeCount;

    [SerializeField] private Transform _container;
    [SerializeField] private List<Potion> _potionPrefabs;

    private Dictionary<DiseaseType, List<Potion>> _potions = new Dictionary<DiseaseType, List<Potion>>();

    private void Awake()
    {
        SpawnPool();
    }

    public Potion TryGetDisabledPotion(DiseaseType diseaseType)
    {
        return _potions[diseaseType].FirstOrDefault(potion => potion.gameObject.activeSelf == false);
    }

    private void SpawnPool()
    {
        Potion spawnedPotion;

        for (int i = 0; i < _potionPrefabs.Count; i++)
        {
            DiseaseType diseaseType = _potionPrefabs[i].GetComponent<Potion>().DiseaseType;
            _potions[diseaseType] = new List<Potion>();

            for (int j = 0; j < _eachPotionTypeCount; j++)
            {
                spawnedPotion = Instantiate(_potionPrefabs[i], _container);
                spawnedPotion.gameObject.SetActive(false);

                _potions[diseaseType].Add(spawnedPotion);
            }
        }
    }
}