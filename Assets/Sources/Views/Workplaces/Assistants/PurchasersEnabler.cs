using UnityEngine;

public class PurchasersEnabler : MonoBehaviour
{
    [SerializeField] private DayTimeCounter _dayTimeCounter;

    [SerializeField] private RoomPurchaser _scabiesPurchaser;
    [SerializeField] private RoomPurchaser _typhusPurchaser;
    [SerializeField] private RoomPurchaser _plaguePurchaser;
    [SerializeField] private RoomPurchaser _depressionPurchaser;

    private bool _isTyphusAdded = false;
    private bool _isScabiesPurchased = false;
    private bool _isPlagueAdded = false;
    private bool _isTyphusPurchased = false;
    private bool _isDepressionAdded = false;
    private bool _isPlaguePurchased = false;

    private void OnEnable()
    {
        _dayTimeCounter.DiseaseAdded += OnDiseaseAdded;

        _scabiesPurchaser.Purchased += OnScabiesPurchased;
        _typhusPurchaser.Purchased += OnTyphusPurchased;
        _plaguePurchaser.Purchased += OnPlaguePurchased;
    }

    private void OnDisable()
    {
        _dayTimeCounter.DiseaseAdded -= OnDiseaseAdded;

        _scabiesPurchaser.Purchased -= OnScabiesPurchased;
        _typhusPurchaser.Purchased -= OnTyphusPurchased;
        _plaguePurchaser.Purchased -= OnPlaguePurchased;
    }

    private void Start()
    {
        _scabiesPurchaser.gameObject.SetActive(false);
        _typhusPurchaser.gameObject.SetActive(false);
        _plaguePurchaser.gameObject.SetActive(false);
        _depressionPurchaser.gameObject.SetActive(false);
    }

    private void OnDiseaseAdded(DiseaseType diseaseType)
    {
        switch (diseaseType)
        {
            case DiseaseType.Scabies:
                _scabiesPurchaser.gameObject.SetActive(true);
                break;

            case DiseaseType.Typhus:
                _isTyphusAdded = true;
                TryActivateTyphusZonePurchaser();
                break;

            case DiseaseType.Plague:
                _isPlagueAdded = true;
                TryActivatePlagueZonePurchaser();
                break;

            case DiseaseType.Depression:
                _isDepressionAdded = true;
                TryActivateDepressionZonePurchaser();
                break;
        }
    }

    private void OnScabiesPurchased()
    {
        _isScabiesPurchased = true;
        TryActivateTyphusZonePurchaser();
    }

    private void OnTyphusPurchased()
    {
        _isTyphusPurchased = true;
        TryActivatePlagueZonePurchaser();
    }

    private void OnPlaguePurchased()
    {
        _isPlaguePurchased = true;
        TryActivateDepressionZonePurchaser();
    }

    private void TryActivateTyphusZonePurchaser()
    {
        if (_isTyphusAdded && _isScabiesPurchased)
        {
            _typhusPurchaser.gameObject.SetActive(true);
        }
    }

    private void TryActivatePlagueZonePurchaser()
    {
        if (_isPlagueAdded && _isTyphusPurchased)
        {
            _plaguePurchaser.gameObject.SetActive(true);
        }
    }

    private void TryActivateDepressionZonePurchaser()
    {
        if (_isDepressionAdded && _isPlaguePurchased)
        {
            _depressionPurchaser.gameObject.SetActive(true);
        }
    }
}