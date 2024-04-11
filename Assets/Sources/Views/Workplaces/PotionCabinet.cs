using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PotionCabinetDroper))]
public class PotionCabinet : MonoBehaviour
{
    [SerializeField] private ImmediateActivator _activator;

    [SerializeField] private Transform _influenzaShelf;
    [SerializeField] private Transform _scabiesShelf;
    [SerializeField] private Transform _typhusShelf;
    [SerializeField] private Transform _plagueShelf;
    [SerializeField] private Transform _depressionShelf;

    private PotionCabinetDroper _potionDroper;

    private List<KeeperPlace> _influenzaKeepers;
    private List<KeeperPlace> _scabiesKeepers;
    private List<KeeperPlace> _typhusKeepers;
    private List<KeeperPlace> _plagueKeepers;
    private List<KeeperPlace> _depressionKeepers;

    private void Awake()
    {
        _potionDroper = GetComponent<PotionCabinetDroper>();

        _influenzaKeepers = GetKeepersFromShelf(_influenzaShelf);
        _scabiesKeepers = GetKeepersFromShelf(_scabiesShelf);
        _typhusKeepers = GetKeepersFromShelf(_typhusShelf);
        _plagueKeepers = GetKeepersFromShelf(_plagueShelf);
        _depressionKeepers = GetKeepersFromShelf(_depressionShelf);
    }

    private void OnEnable()
    {
        _activator.PlayerEntered += OnPlayerEntered;

        _potionDroper.InfluenzaActivated += OnInfluenzaActivated;
        _potionDroper.ScabiesActivated += OnScabiesActivated;
        _potionDroper.TyphusActivated += OnTyphusActivated;
        _potionDroper.PlagueActivated += OnPlagueActivated;
        _potionDroper.DepressionActivated += OnDepressionActivated;
    }

    private void OnDisable()
    {
        _activator.PlayerEntered -= OnPlayerEntered;

        _potionDroper.InfluenzaActivated -= OnInfluenzaActivated;
        _potionDroper.ScabiesActivated -= OnScabiesActivated;
        _potionDroper.TyphusActivated -= OnTyphusActivated;
        _potionDroper.PlagueActivated -= OnPlagueActivated;
        _potionDroper.DepressionActivated -= OnDepressionActivated;
    }

    private void Start()
    {
        TryEnableUnemptyShelvesActivators(_influenzaKeepers, DiseaseType.Influenza);
        TryEnableUnemptyShelvesActivators(_scabiesKeepers, DiseaseType.Scabies);
        TryEnableUnemptyShelvesActivators(_typhusKeepers, DiseaseType.Typhus);
        TryEnableUnemptyShelvesActivators(_plagueKeepers, DiseaseType.Plague);
        TryEnableUnemptyShelvesActivators(_depressionKeepers, DiseaseType.Depression);
    }

    private void TryEnableUnemptyShelvesActivators(List<KeeperPlace> keepers, DiseaseType diseaseType)
    {
        if (keepers.Where(keeper => keeper.IsEmpty == false).Count() != 0)
        {
            _potionDroper.TryEnableActivator(diseaseType);
        }
    }

    private void OnPlayerEntered(Player player)
    {
        List<KeeperPlace> playerKeepersWithPotions = player.TryGetKeepersWithObjects()
            .Where(keeper => keeper.ShowObjectInKeep().TryGetComponent(out Potion potion)).ToList();

        foreach (var playerKeeperWithPotion in playerKeepersWithPotions)
        {
            DiseaseType diseaseType = playerKeeperWithPotion.ShowObjectInKeep().GetComponent<Potion>().DiseaseType;

            List<KeeperPlace> keepers = SwitchKeeperByDisease(diseaseType);
            KeeperPlace[] emptyKeepers = keepers.Where(keeper => keeper.IsEmpty).ToArray();

            if (emptyKeepers.Length != 0)
            {
                player.GiveAway(playerKeeperWithPotion, emptyKeepers.First());
                _potionDroper.TryEnableActivator(diseaseType);
            }
        }
    }

    private void OnInfluenzaActivated(Player player)
    {
        TryExchangePotionFromShelf(player, _influenzaKeepers, DiseaseType.Influenza);
    }

    private void OnScabiesActivated(Player player)
    {
        TryExchangePotionFromShelf(player, _scabiesKeepers, DiseaseType.Scabies);
    }

    private void OnTyphusActivated(Player player)
    {
        TryExchangePotionFromShelf(player, _typhusKeepers, DiseaseType.Typhus);
    }

    private void OnPlagueActivated(Player player)
    {
        TryExchangePotionFromShelf(player, _plagueKeepers, DiseaseType.Plague);
    }

    private void OnDepressionActivated(Player player)
    {
        TryExchangePotionFromShelf(player, _depressionKeepers, DiseaseType.Depression);
    }

    private void TryExchangePotionFromShelf(Player player, List<KeeperPlace> keepers, DiseaseType diseaseType)
    {
        KeeperPlace[] unemptyKeepers = keepers.Where(keeper => keeper.IsEmpty == false).ToArray();

        player.TryTake(unemptyKeepers);

        unemptyKeepers = keepers.Where(keeper => keeper.IsEmpty == false).ToArray();

        if (unemptyKeepers.Length == 0)
        {
            _potionDroper.TryDisableActivator(diseaseType);
        }
    }

    private List<KeeperPlace> SwitchKeeperByDisease(DiseaseType diseaseType)
    {
        List<KeeperPlace> keepers = new List<KeeperPlace>();

        switch(diseaseType)
        {
            case DiseaseType.Influenza:
                keepers = _influenzaKeepers;
                break;

            case DiseaseType.Scabies:
                keepers = _scabiesKeepers;
                break;

            case DiseaseType.Typhus:
                keepers = _typhusKeepers;
                break;

            case DiseaseType.Plague:
                keepers = _plagueKeepers;
                break;

            case DiseaseType.Depression:
                keepers = _depressionKeepers;
                break;
        }

        return keepers;
    }

    private List<KeeperPlace> GetKeepersFromShelf(Transform shelf)
    {
        return shelf.GetComponentsInChildren<KeeperPlace>().ToList();
    } 
}