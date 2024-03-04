using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Wallet))]
public class WalletView : MonoBehaviour
{
    private const int MoneyChangeValue = 1;

    [SerializeField] private TMP_Text _moneyText;

    private Wallet _wallet;
    private Coroutine _moneyViewChanger;

    private int _currentMoneyViewValue;

    private void Awake()
    {
        _wallet = GetComponent<Wallet>();
    }

    private void OnEnable()
    {
        _wallet.Initialized += OnWalletInitialized;
        _wallet.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _wallet.Initialized -= OnWalletInitialized;
        _wallet.MoneyChanged -= OnMoneyChanged;
    }

    private void OnWalletInitialized(int money)
    {
        _currentMoneyViewValue = money;
        _moneyText.text = _currentMoneyViewValue.ToString();
    }

    private void OnMoneyChanged(int money)
    {
        if (_moneyViewChanger != null)
        {
            StopCoroutine(_moneyViewChanger);
        }

        _moneyViewChanger = StartCoroutine(ChangeMoneyView(money));
    }

    private IEnumerator ChangeMoneyView(int targetMoneyValue)
    {
        while (_currentMoneyViewValue != targetMoneyValue)
        {
            _currentMoneyViewValue = (int)Mathf.MoveTowards(_currentMoneyViewValue, targetMoneyValue, MoneyChangeValue);

            _moneyText.text = _currentMoneyViewValue.ToString();

            yield return null;
        }

        _moneyViewChanger = null;
    }
}
