using System.Linq;
using UnityEngine;

public class PotionCabinet : MonoBehaviour
{
    [SerializeField] private DiseaseType _diseaseType;
    [SerializeField] private ImmediateActivator _activator;
    [SerializeField] private KeeperPlace[] _keepers;

    private void OnEnable()
    {
        _activator.PlayerEntered += OnPlayerEntered;
    }

    private void OnDisable()
    {
        _activator.PlayerEntered -= OnPlayerEntered;
    }

    private void Start()
    {
        TryChangeActivatorAvailability();
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

        TryChangeActivatorAvailability();
    }

    private void TryChangeActivatorAvailability()
    {
        if (_keepers.Where(keeper => keeper.IsEmpty).Count() == 0)
        {
            _activator.gameObject.SetActive(false);
        }
        else
        {
            _activator.gameObject.SetActive(true);
        }
    }
}
