using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class ActivatorWithDelay : Activator
{
    private const float WaitingSeconds = 0.5f;

    [SerializeField] private int _delay;

    private Coroutine _activatorWithDelay = null;

    public event UnityAction<Player> Activated;
    public event UnityAction<int> Counted;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            TryFinishActivating();

            _activatorWithDelay = null;
        }
    }

    protected override void Activate(Player player)
    {
        TryFinishActivating();

        _activatorWithDelay = StartCoroutine(ActivateWithDelay(player));
    }

    private IEnumerator ActivateWithDelay(Player player)
    {
        int counter = 0;
        WaitForSecondsRealtime waitingTime = new WaitForSecondsRealtime(WaitingSeconds);

        do
        {
            Counted?.Invoke(counter);
            counter++;

            yield return waitingTime;
        }
        while (counter < _delay);

        Activated?.Invoke(player);
        _activatorWithDelay = null;
    }

    private void TryFinishActivating()
    {
        if (_activatorWithDelay != null)
        {
            StopCoroutine(_activatorWithDelay);
        }
    }
}
