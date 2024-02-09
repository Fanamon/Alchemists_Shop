using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _startingMoneyCount;

    private int _money;

    public event UnityAction<int> MoneyChanged;

    private void Awake()
    {
        _money = _startingMoneyCount;
        MoneyChanged?.Invoke(_money);
    }

    public void Take(int money)
    {
        _money += money;
        MoneyChanged?.Invoke(_money);
    }

    private void OnValidate()
    {
        if (_startingMoneyCount < 0)
        {
            _startingMoneyCount = 0;
        }
    }
}
