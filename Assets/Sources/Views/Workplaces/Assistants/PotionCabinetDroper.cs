using UnityEngine;
using UnityEngine.Events;

public class PotionCabinetDroper : MonoBehaviour
{
    [SerializeField] private ActivatorWithDelay _influenzaActivator;
    [SerializeField] private ActivatorWithDelay _scabiesActivator;
    [SerializeField] private ActivatorWithDelay _typhusActivator;
    [SerializeField] private ActivatorWithDelay _plagueActivator;
    [SerializeField] private ActivatorWithDelay _depressionActivator;

    public event UnityAction<Player> InfluenzaActivated;
    public event UnityAction<Player> ScabiesActivated;
    public event UnityAction<Player> TyphusActivated;
    public event UnityAction<Player> PlagueActivated;
    public event UnityAction<Player> DepressionActivated;

    private void Awake()
    {
        _influenzaActivator.gameObject.SetActive(false);
        _scabiesActivator.gameObject.SetActive(false);
        _typhusActivator.gameObject.SetActive(false);
        _plagueActivator.gameObject.SetActive(false);
        _typhusActivator.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _influenzaActivator.Activated += OnInfluenzaActivated;
        _scabiesActivator.Activated += OnScabiesActivated;
        _typhusActivator.Activated += OnTyphusActivated;
        _plagueActivator.Activated += OnPlagueActivated;
        _depressionActivator.Activated += OnDepressionActivated;
    }

    private void OnDisable()
    {
        _influenzaActivator.Activated -= OnInfluenzaActivated;
        _scabiesActivator.Activated -= OnScabiesActivated;
        _typhusActivator.Activated -= OnTyphusActivated;
        _plagueActivator.Activated -= OnPlagueActivated;
        _depressionActivator.Activated -= OnDepressionActivated;
    }

    public void TryEnableActivator(DiseaseType diseaseType)
    {
        ActivatorWithDelay activator = SwitchActivator(diseaseType);

        if (activator.gameObject.activeSelf == false)
        {
            activator.gameObject.SetActive(true);
        }
    }

    public void TryDisableActivator(DiseaseType diseaseType)
    {
        ActivatorWithDelay activator = SwitchActivator(diseaseType);

        activator.gameObject.SetActive(false);
    }

    private ActivatorWithDelay SwitchActivator(DiseaseType diseaseType)
    {
        ActivatorWithDelay activator = null;

        switch (diseaseType)
        {
            case DiseaseType.Influenza:
                activator = _influenzaActivator;
                break;

            case DiseaseType.Scabies:
                activator = _scabiesActivator;
                break;

            case DiseaseType.Typhus:
                activator = _typhusActivator;
                break;

            case DiseaseType.Plague:
                activator = _plagueActivator;
                break;

            case DiseaseType.Depression:
                activator = _depressionActivator;
                break;
        }

        return activator;
    }

    private void OnInfluenzaActivated(Player player)
    {
        InfluenzaActivated?.Invoke(player);
    }

    private void OnScabiesActivated(Player player)
    {
        ScabiesActivated?.Invoke(player);
    }

    private void OnTyphusActivated(Player player)
    {
        TyphusActivated?.Invoke(player);
    }

    private void OnPlagueActivated(Player player)
    {
        PlagueActivated?.Invoke(player);
    }

    private void OnDepressionActivated(Player player)
    {
        DepressionActivated?.Invoke(player);
    }
}