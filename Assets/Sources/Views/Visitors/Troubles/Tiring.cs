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

    public void StartTiring()
    {
        if (_tiring != null)
        {
            StopTiring();
        }

        _tiring = StartCoroutine(Tire());
    }

    public void StopTiring()
    {
        StopCoroutine(_tiring);
    }

    private IEnumerator Tire()
    {
        float tiringTime = Random.Range(_minTiringTimeInMinutes, _maxTiringTimeInMinutes) * 
            MinutesToSecondsMultiplier;

        yield return new WaitForSecondsRealtime(tiringTime);

        Tired?.Invoke();
        _tiring = null;
    }
}
