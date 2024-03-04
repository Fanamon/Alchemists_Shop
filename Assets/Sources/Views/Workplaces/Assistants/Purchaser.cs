using UnityEngine;
using UnityEngine.Events;

public abstract class Purchaser : MonoBehaviour
{
    [SerializeField] private int _upgradeCost;
    
    [SerializeField] private ImmediateActivator _activator;
    [SerializeField] private Transform _placeToPurchasing;

    private int _paidMoney;

    public event UnityAction<int> MoneyPaid;

    private void OnEnable()
    {
        _activator.PlayerEntered += OnPlayerEntered;
    }

    private void OnDisable()
    {
        _activator.PlayerEntered -= OnPlayerEntered;
    }

    private void Start()
    {
        _paidMoney = 0;
        Initialize(_placeToPurchasing);
    }

    public virtual void Purchase()
    {
        _activator.gameObject.SetActive(false);
    }

    private void OnPlayerEntered(Player player)
    {
        Wallet playersWallet = player.GetComponent<Wallet>();

        _paidMoney += playersWallet.Pay(_upgradeCost - _paidMoney);
        MoneyPaid?.Invoke(_upgradeCost - _paidMoney);

        if (_paidMoney == _upgradeCost)
        {
            Purchase();
        }
    }

    protected abstract void Initialize(Transform placeToPurchasing);
}