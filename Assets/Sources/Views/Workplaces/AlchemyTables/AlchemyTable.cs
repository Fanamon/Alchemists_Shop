using System.Collections;
using System.Linq;
using UnityEngine;

public class AlchemyTable : MonoBehaviour
{
    [SerializeField] private float _makingPotionTime;

    [SerializeField] private DiseaseType _diseaseType;

    [SerializeField] private ImmediateActivator _activator;
    [SerializeField] private ImmediateActivator _zoneActivator;
    [SerializeField] private PotionPool _pool;
    [SerializeField] private KeeperPlace[] _keepers;
    [SerializeField] private ReagentPlace[] _reagentKeepers;

    private void OnEnable()
    {
        _activator.PlayerEntered += OnPlayerEntered;
        _zoneActivator.PlayerEntered += OnZonePlayerEntered;
    }

    private void OnDisable()
    {
        _activator.PlayerEntered -= OnPlayerEntered;
        _zoneActivator.PlayerEntered -= OnZonePlayerEntered;
    }

    private void Start()
    {
        if (_keepers.Where(keeper => keeper.IsEmpty == false).Count() == 0)
        {
            _activator.gameObject.SetActive(false);
        }
        else
        {
            _activator.gameObject.SetActive(true);
        }

        TryDisableZoneActivator();
        TryMakePotions();
    }

    private void OnPlayerEntered(Player player)
    {
        KeeperPlace[] unemptyKeepers = _keepers.Where(keeper => keeper.IsEmpty == false).ToArray();

        player.TryTake(unemptyKeepers);

        if (_keepers.Where(keeper => keeper.IsEmpty == false).Count() == 0)
        {
            _activator.gameObject.SetActive(false);
        }

        TryMakePotions();
    }

    private void OnZonePlayerEntered(Player player)
    {
        KeeperPlace[] playerKeepers = player.TryGetKeepersWithObjects()
            .Where(keeper => keeper.ShowObjectInKeep().TryGetComponent(out Reagent reagent)).ToArray();
        ReagentPlace[] emptyKeepers = _reagentKeepers.Where(keeper => keeper.IsEmpty).ToArray();
        ReagentPlace[] unemptyKeepers = _reagentKeepers.Where(keeper => keeper.IsEmpty == false).ToArray();

        if (playerKeepers.Length != 0 && emptyKeepers.Length != 0)
        {
            int minLength = System.Math.Min(playerKeepers.Length, emptyKeepers.Length);

            for (int i = 0; i < minLength; i++)
            {
                player.GiveAway(playerKeepers.First(keeperWithReagent => keeperWithReagent.IsEmpty == false),
                    emptyKeepers[i]);
            }

            TryMakePotions();
        }

        TryDisableZoneActivator();
    }

    private void TryMakePotions()
    {
        KeeperPlace[] emptyPotionKeepers = _keepers.Where(keeper => keeper.IsEmpty).ToArray();
        ReagentPlace[] placesWithReagents = _reagentKeepers.Where(keeper => keeper.IsEmpty == false &&
        keeper.IsProcessing == false).ToArray();

        if (emptyPotionKeepers.Length != 0 && placesWithReagents.Length != 0)
        {
            StartCoroutine(MakePotions(placesWithReagents, emptyPotionKeepers));
        }
    }

    private void TryDisableZoneActivator()
    {
        if (_reagentKeepers.Where(keeper => keeper.IsEmpty).Count() == 0)
        {
            _zoneActivator.gameObject.SetActive(false);
        }
    }

    private IEnumerator MakePotions(ReagentPlace[] placesWithReagents, KeeperPlace[] emptyPotionKeepers)
    {
        int minLength = System.Math.Min(placesWithReagents.Length, emptyPotionKeepers.Length);
        placesWithReagents.Take(minLength).ToList().ForEach(keeper => keeper.StartProcessing());

        WaitForSeconds waitingTime = new WaitForSeconds(_makingPotionTime);

        yield return waitingTime;

        for (int i = 0; i < minLength; i++)
        {
            CreatePotion(placesWithReagents[i], emptyPotionKeepers[i]);
        }

        if (_zoneActivator.gameObject.activeSelf == false)
        {
            _zoneActivator.gameObject.SetActive(true);
        }

        if (_activator.gameObject.activeSelf == false)
        {
            _activator.gameObject.SetActive(true);
        }
    }

    private void CreatePotion(ReagentPlace placeWithReagent, KeeperPlace emptyPotionKeeper)
    {
        Potion potion = _pool.TryGetDisabledPotion(_diseaseType);

        potion.transform.position = emptyPotionKeeper.transform.position;
        emptyPotionKeeper.Take(potion.transform);
        placeWithReagent.DropObject();
        potion.gameObject.SetActive(true);
    }
}