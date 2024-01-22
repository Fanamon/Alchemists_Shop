using System.Linq;
using UnityEngine;

public class PotionCabinet : MonoBehaviour
{
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

    private void OnPlayerEntered(Player player)
    {
        KeeperPlace[] playerKeepers = player.TryGetKeepersWithObjects();
        KeeperPlace[] emptyKeepers = _keepers.Where(keeper => keeper.IsEmpty).ToArray();

        playerKeepers = playerKeepers.Where(keeper => keeper.ShowObjectInKeep().
        TryGetComponent(out Potion potion)).ToArray();

        if (playerKeepers.Length == 0 || emptyKeepers.Length == 0)
        {
            return;
        }

        foreach (var keeper in emptyKeepers)
        {
            player.GiveAway(playerKeepers.First(keeperWithPotion => keeperWithPotion.IsEmpty == false), 
                keeper);
        }
    }
}
