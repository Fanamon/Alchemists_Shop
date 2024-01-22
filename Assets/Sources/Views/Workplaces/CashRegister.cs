using UnityEngine;

public class CashRegister : MonoBehaviour
{
    [SerializeField] private ActivatorWithDelay _activator;
    [SerializeField] private GameObject _activateablePlace;

    private void Awake()
    {
        _activateablePlace.SetActive(false);
    }

    private void OnEnable()
    {
        _activator.Activated += OnActivated;
    }

    private void OnDisable()
    {
        _activator.Activated -= OnActivated;
    }

    private void OnActivated()
    {
        _activateablePlace.SetActive(true);
    }
}
