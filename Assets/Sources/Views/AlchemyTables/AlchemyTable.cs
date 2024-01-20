using System.Linq;
using UnityEngine;

public class AlchemyTable : MonoBehaviour
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
        player.TryTake(_keepers.Where(keeper => keeper.IsEmpty == false).ToArray());
    } 
}