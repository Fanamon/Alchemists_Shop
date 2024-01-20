using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private KeeperPlace[] _keepers;

    public void TryTake(IKeepable[] keepersWithObject)
    {
        if (keepersWithObject.Length == 0)
        {
            return;
        }

        foreach (var keeper in _keepers)
        {
            if (keeper.IsEmpty)
            {
                keeper.Take(keepersWithObject.First(keeperWithObject => keeperWithObject.IsEmpty == false).
                    GiveAway());
            }
        }
    }

    public KeeperPlace[] TryGetKeepersWithObjects()
    {
        return _keepers.Where(keeper => keeper.IsEmpty == false).ToArray();
    }

    public void GiveAway(KeeperPlace fromKeeper, KeeperPlace toKeeper)
    {
        toKeeper.Take(fromKeeper.GiveAway());
    }
}