using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionCabinet : Workplace
{
    private const int PlacesForUpgradeCount = 3;

    [SerializeField] private DiseaseType _diseaseType;
    [SerializeField] private Transform _placesForUpgradesKeeper;
    [SerializeField] private ImmediateActivator _activator;
    [SerializeField] private List<KeeperPlace> _keepers;

    private List<KeeperPlace> _additionalPlacesForUpgrades;

    private void Awake()
    {
        _additionalPlacesForUpgrades = _placesForUpgradesKeeper.GetComponentsInChildren<KeeperPlace>(true).ToList();
        _additionalPlacesForUpgrades.ForEach(place => place.gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        _activator.PlayerEntered += OnPlayerEntered;
    }

    private void OnDisable()
    {
        _activator.PlayerEntered -= OnPlayerEntered;
    }

    public void Initialize(DiseaseType diseaseType)
    {
        _diseaseType = diseaseType;
    }

    public override void Upgrade()
    {
        List<KeeperPlace> placesToAdd = _additionalPlacesForUpgrades.Take(PlacesForUpgradeCount).ToList();

        placesToAdd.ForEach(place => place.gameObject.SetActive(true));
        placesToAdd.ForEach(place =>
        {
            _additionalPlacesForUpgrades.Remove(place);
            _keepers.Add(place);
        });
    }

    private void OnPlayerEntered(Player player)
    {
        KeeperPlace[] playerKeepers = player.TryGetKeepersWithObjects()
            .Where(keeper => keeper.ShowObjectInKeep().TryGetComponent(out Potion potion) && 
            keeper.ShowObjectInKeep().GetComponent<Potion>().DiseaseType == _diseaseType).ToArray();
        KeeperPlace[] emptyKeepers = _keepers.Where(keeper => keeper.IsEmpty).ToArray();
        KeeperPlace[] unemptyKeepers = _keepers.Where(keeper => keeper.IsEmpty == false).ToArray();

        if (playerKeepers.Length != 0 && emptyKeepers.Length != 0)
        {
            int minLength = System.Math.Min(playerKeepers.Length, emptyKeepers.Length);

            for (int i = 0; i < minLength; i++)
            {
                player.GiveAway(playerKeepers.First(keeperWithPotion => keeperWithPotion.IsEmpty == false),
                    emptyKeepers[i]);
            }
        }
        else if (unemptyKeepers.Length != 0)
        {
            player.TryTake(unemptyKeepers);
        }
    }
}
