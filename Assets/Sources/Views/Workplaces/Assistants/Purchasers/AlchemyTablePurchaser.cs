using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlchemyTablePurchaser : Purchaser
{
    [SerializeField] private AlchemyTable _alchemyTablePrefab;
    [SerializeField] private PotionPool _potionPool;

    [SerializeField] private List<PotionsCabinetPurchaser> _potionsCabinetPurchasers;

    [SerializeField] private DiseaseType _diseaseType;
    [SerializeField] private bool _isActive = false;

    private AlchemyTable _alchemyTable;

    public event UnityAction<DiseaseType> Purchased;

    public override void Purchase()
    {
        base.Purchase();
        _alchemyTable.gameObject.SetActive(true);

        foreach (var potionCabinetPurchaser in _potionsCabinetPurchasers)
        {
            potionCabinetPurchaser.gameObject.SetActive(true);
        }

        Purchased?.Invoke(_diseaseType);
    }

    protected override void Initialize(Transform placeToPurchasing)
    {
        _alchemyTable = Instantiate(_alchemyTablePrefab, placeToPurchasing);
        _alchemyTable.Initialize(_potionPool, _diseaseType);

        if (_isActive == false)
        {
            _alchemyTable.gameObject.SetActive(false);

            foreach (var potionCabinetPurchaser in _potionsCabinetPurchasers)
            {
                potionCabinetPurchaser.gameObject.SetActive(false);
            }
        }
        else
        {
            Purchase();
        }
    }
}
