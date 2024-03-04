using UnityEngine;
using UnityEngine.Events;

public class Upgrader : MonoBehaviour
{
    private const float UpgradeCostIncreasePercentage = 0.2f;

    private const int MinUpgradeLevel = 1;
    private const int MaxUpgradeLevel = 3;

    [SerializeField] private int _upgradeCost;

    [SerializeField] private ImmediateActivator _activator;
    [SerializeField] private Workplace _workplaceToUpgrade;

    private int _upgradeLevel;
    private int _paidMoney;

    public event UnityAction<int> MoneyPaid;

    private void Awake()
    {
        _upgradeLevel = MinUpgradeLevel;
        _paidMoney = 0;
    }

    private void OnEnable()
    {
        _activator.PlayerEntered += OnPlayerEntered;
    }

    private void OnDisable()
    {
        _activator.PlayerEntered -= OnPlayerEntered;
    }

    private void OnPlayerEntered(Player player)
    {
        Wallet playersWallet = player.GetComponent<Wallet>();

        _paidMoney += playersWallet.Pay(_upgradeCost - _paidMoney);
        MoneyPaid?.Invoke(_upgradeCost - _paidMoney);

        if (_paidMoney == _upgradeCost)
        {
            _upgradeCost = Mathf.CeilToInt(_upgradeCost * (1 + UpgradeCostIncreasePercentage));
            _paidMoney = 0;
            _workplaceToUpgrade.Upgrade();
            _upgradeLevel++;
        }

        if (_upgradeLevel == MaxUpgradeLevel)
        {
            _activator.gameObject.SetActive(false);
        }
    }
}

public interface IUpgradeable
{
    void Upgrade();
}
