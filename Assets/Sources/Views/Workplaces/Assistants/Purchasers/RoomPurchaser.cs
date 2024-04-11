using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomPurchaser : Purchaser
{
    [SerializeField] private List<WallToDisable> _wallsToDisable;
    [SerializeField] private List<Purchaser> _purchasers;

    [SerializeField] private bool _isActive = false;

    public event UnityAction Purchased;

    public override void Purchase()
    {
        base.Purchase();

        ChangeWallsActiveStatus(false);
        ChangePurchasersActiveStatus(true);

        Purchased?.Invoke();
    }

    protected override void Initialize(Transform placeToPurchasing)
    {
        if (_isActive == false)
        {
            ChangeWallsActiveStatus(true);
            ChangePurchasersActiveStatus(false);
        }
        else
        {
            Purchase();
        }
    }

    private void ChangeWallsActiveStatus(bool status)
    {
        foreach (var wall in _wallsToDisable)
        {
            wall.gameObject.SetActive(status);
        }
    }

    private void ChangePurchasersActiveStatus(bool status)
    {
        foreach (var purchaser in _purchasers)
        {
            purchaser.gameObject.SetActive(status);
        }
    }
}