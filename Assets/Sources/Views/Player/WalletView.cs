using TMPro;
using UnityEngine;

[RequireComponent(typeof(Wallet))]
public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;

    private Wallet _wallet;

    private void Awake()
    {
        _wallet = GetComponent<Wallet>();
    }

    private void OnEnable()
    {
        _wallet.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _wallet.MoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(int money)
    {
        _moneyText.text = money.ToString();
    }
}
