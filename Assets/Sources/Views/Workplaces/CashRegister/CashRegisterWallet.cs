using System.Collections.Generic;
using UnityEngine;

public class CashRegisterWallet : MonoBehaviour
{
    [SerializeField] private QueueGenerator _generator;
    [SerializeField] private ImmediateActivator _walletActivator;

    private int _money;
    private List<Visitor> _visitorsInQueue = new List<Visitor>();

    private void Awake()
    {
        ResetMoney();
    }

    private void OnEnable()
    {
        _generator.VisitorGenerated += OnVisitorGenerated;
        _walletActivator.PlayerEntered += OnPlayerEntered;
    }

    private void OnDisable()
    {
        _generator.VisitorGenerated -= OnVisitorGenerated;
        _walletActivator.PlayerEntered -= OnPlayerEntered;

        foreach (var visitor in _visitorsInQueue)
        {
            if (visitor != null)
            {
                visitor.GetComponent<PotionDrinker>().Cured -= OnVisitorCured;
                visitor.Left -= OnVisitorLeft;
            }
        }
    }

    private void ResetMoney()
    {
        _money = 0;
        _walletActivator.gameObject.SetActive(false);
    }

    private void OnVisitorGenerated(Visitor visitor)
    {
        visitor.GetComponent<PotionDrinker>().Cured += OnVisitorCured;
        visitor.Left += OnVisitorLeft; 
        _visitorsInQueue.Add(visitor);
    }

    private void OnVisitorCured(PotionDrinker potionDrinker)
    {
        OnVisitorLeft(potionDrinker.GetComponent<Visitor>());
        _money += potionDrinker.GetCountedReward();
        _walletActivator.gameObject.SetActive(true);
    }

    private void OnVisitorLeft(Visitor visitor)
    {
        visitor.GetComponent<PotionDrinker>().Cured -= OnVisitorCured;
        visitor.Left -= OnVisitorLeft;
        _visitorsInQueue.Remove(visitor);
    }

    private void OnPlayerEntered(Player player)
    {
        player.GetComponent<Wallet>().Take(_money);
        ResetMoney();
    }
}