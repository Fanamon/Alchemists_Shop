using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Disease))]
[RequireComponent(typeof(Tiring))]
public class PotionDrinker : MonoBehaviour
{
    private const float GuarantedPaymentPart = 0.3f;

    [SerializeField] private int _minRewardValue = 10;
    [SerializeField] private int _maxRewardValue = 25;

    private int _rewardValue;
    private Disease _disease;
    private Tiring _tiring;

    public event UnityAction<PotionDrinker> Cured;
    public event UnityAction Failed;

    private void Awake()
    {
        _disease = GetComponent<Disease>();
        _tiring = GetComponent<Tiring>();
    }

    private void OnEnable()
    {
        _rewardValue = Random.Range(_minRewardValue, _maxRewardValue + 1);
    }

    public void Drink(Potion potion, float drinkingTime)
    {
        StartCoroutine(TakeIn(potion, drinkingTime));
    }

    public int GetCountedReward()
    {
        float paymentPart = _tiring.TiringCounter / _tiring.TiringTime;
        paymentPart = Mathf.Max(paymentPart, GuarantedPaymentPart);

        return (int)System.Math.Ceiling(paymentPart * _rewardValue);
    }

    private void OnValidate()
    {
        if (_maxRewardValue < _minRewardValue)
        {
            int tempValue = _minRewardValue;
            _minRewardValue = _maxRewardValue;
            _maxRewardValue = tempValue;
        }

        if (_minRewardValue < 0 || _maxRewardValue < 0)
        {
            _minRewardValue = Mathf.Max(0, _minRewardValue);
            _maxRewardValue = Mathf.Max(0, _maxRewardValue);
        }
    }

    private IEnumerator TakeIn(Potion potion, float drinkingTime)
    {
        WaitForSeconds waitingTime = new WaitForSeconds(drinkingTime);

        yield return waitingTime;

        if (potion.DiseaseType == _disease.Type)
        {
            GetCountedReward();
            Cured?.Invoke(this);
        }
        else
        {
            Failed?.Invoke();
        }
    }
}
