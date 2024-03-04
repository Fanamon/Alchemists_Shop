using UnityEngine;

public class ReagentCreatorPurchaser : Purchaser
{
    [SerializeField] private ReagentCreator _reagentCreatorPrefab;
    [SerializeField] private ReagentPool _reagentPool;

    [SerializeField] private bool _isActive = false;

    private ReagentCreator _reagentCreator;

    public override void Purchase()
    {
        base.Purchase();
        _reagentCreator.gameObject.SetActive(true);
    }

    protected override void Initialize(Transform placeToPurchasing)
    {
        _reagentCreator = Instantiate(_reagentCreatorPrefab, placeToPurchasing);
        _reagentCreator.Initialize(_reagentPool);
        
        if (_isActive == false)
        {
            _reagentCreator.gameObject.SetActive(false);
        }
        else
        {
            Purchase();
        }
    }
}
