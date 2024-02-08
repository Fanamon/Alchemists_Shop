using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private KeeperPlace[] _keepers;

    public void TryTake(IKeepable[] keepersWithObject)
    {
        KeeperPlace[] emptyHands = _keepers.Where(hand => hand.IsEmpty).ToArray();

        if (keepersWithObject.Length != 0 && emptyHands.Length != 0)
        {
            int minLength = System.Math.Min(keepersWithObject.Length, emptyHands.Length);

            for (int i = 0; i < minLength; i++)
            {
                emptyHands[i].Take(keepersWithObject.First(keeperWithObject => keeperWithObject.IsEmpty == false).
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