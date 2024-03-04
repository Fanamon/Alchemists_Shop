using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _startingMoneyCount;

    private int _money;

    public event UnityAction<int> Initialized;
    public event UnityAction<int> MoneyChanged;

    private void Awake()
    {
        _money = _startingMoneyCount;
        Initialized?.Invoke(_money);
    }

    public void Take(int money)
    {
        _money += money;
        MoneyChanged?.Invoke(_money);
    }

    public int Pay(int cost)
    {
        int moneyToPay = _money;

        if (cost >= _money)
        {
            _money = 0;
        }
        else
        {
            moneyToPay = cost;
            _money -= cost;
        }

        MoneyChanged?.Invoke(_money);

        return moneyToPay;
    }

    private void OnValidate()
    {
        if (_startingMoneyCount < 0)
        {
            _startingMoneyCount = 0;
        }
    }
}
