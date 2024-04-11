using UnityEngine;
using UnityEngine.Events;

public class AlchemyTablePurchaser : Purchaser
{
    [SerializeField] private AlchemyTable _alchemyTablePrefab;
    [SerializeField] private PotionPool _potionPool;

    [SerializeField] private bool _isActive = false;

    private AlchemyTable _alchemyTable;

    public event UnityAction Purchased;

    public override void Purchase()
    {
        base.Purchase();
        _alchemyTable.gameObject.SetActive(true);

        Purchased?.Invoke();
    }

    protected override void Initialize(Transform placeToPurchasing)
    {
        _alchemyTable = Instantiate(_alchemyTablePrefab, placeToPurchasing);
        _alchemyTable.Initialize(_potionPool);

        if (_isActive == false)
        {
            _alchemyTable.gameObject.SetActive(false);
        }
        else
        {
            Purchase();
        }
    }
}
