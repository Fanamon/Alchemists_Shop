using UnityEngine;

public class PotionsCabinetPurchaser : Purchaser
{
    [SerializeField] private PotionCabinet _potionCabinetPrefab;

    private PotionCabinet _potionCabinet;

    public override void Purchase()
    {
        base.Purchase();
        _potionCabinet.gameObject.SetActive(true);
    }

    protected override void Initialize(Transform placeToPurchasing)
    {
        _potionCabinet = Instantiate(_potionCabinetPrefab, placeToPurchasing);
        _potionCabinet.gameObject.SetActive(false);
    }
}
