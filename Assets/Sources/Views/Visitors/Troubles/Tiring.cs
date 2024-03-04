using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Tiring : MonoBehaviour
{
    private const float MinutesToSecondsMultiplier = 60;

    [SerializeField] private float _minTiringTimeInMinutes = 0.7f;
    [SerializeField] private float _maxTiringTimeInMinutes = 1.4f;

    private Coroutine _tiring = null;

    public event UnityAction Tired;

    public float TiringTime { get; private set; }
    public float TiringCounter { get; private set; }

    public void StartTiring()
    {
        StopTiring();

        _tiring = StartCoroutine(Tire());
    }

    public void StopTiring()
    {
        if (_tiring != null)
        {
            StopCoroutine(_tiring);
        }
    }

    private IEnumerator Tire()
    {
        TiringTime = Random.Range(_minTiringTimeInMinutes, _maxTiringTimeInMinutes) * 
            MinutesToSecondsMultiplier;
        TiringCounter = 0;

        while (TiringCounter < TiringTime)
        {
            yield return null;

            TiringCounter += Time.deltaTime;
        }

        Tired?.Invoke();
        _tiring = null;
    }

    private void OnValidate()
    {
        if (_maxTiringTimeInMinutes < _minTiringTimeInMinutes)
        {
            float tempValue = _maxTiringTimeInMinutes;
            _maxTiringTimeInMinutes = _minTiringTimeInMinutes;
            _minTiringTimeInMinutes = tempValue;
        }

        if (_maxTiringTimeInMinutes < 0 || _minTiringTimeInMinutes < 0)
        {
            _minTiringTimeInMinutes = Mathf.Max(0, _minTiringTimeInMinutes);
            _maxTiringTimeInMinutes = Mathf.Max(0, _maxTiringTimeInMinutes);
        }
    }
}
