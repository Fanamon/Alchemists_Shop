using System.Collections;
using System.Linq;
using UnityEngine;

public class ReagentCreator : Workplace
{
    private const float MakingReagentTimeDecreaseByUpgradePercentage = 0.25f;

    [SerializeField] private float _startingMakingReagentTime;

    [SerializeField] private ImmediateActivator _activator;
    [SerializeField] private ReagentPool _pool;
    [SerializeField] private KeeperPlace[] _keepers;

    private float _makingReagentTime;

    private Coroutine _reagentMaker = null;

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
        _makingReagentTime = _startingMakingReagentTime;

        if (_keepers.Where(keeper => keeper.IsEmpty).Count() != 0)
        {
            _reagentMaker = StartCoroutine(MakeReagents());
        }
        else if (_keepers.Where(keeper => keeper.IsEmpty == false).Count() == 0)
        {
            _activator.gameObject.SetActive(false);
        }
    }

    public void Initialize(ReagentPool pool)
    {
        _pool = pool;
    }

    public override void Upgrade()
    {
        _makingReagentTime *= (1 - MakingReagentTimeDecreaseByUpgradePercentage);
    }

    private void OnPlayerEntered(Player player)
    {
        player.TryTake(_keepers.Where(keeper => keeper.IsEmpty == false).ToArray());

        if (_reagentMaker == null)
        {
            _reagentMaker = StartCoroutine(MakeReagents());
        }

        if (_keepers.Where(keeper => keeper.IsEmpty == false).Count() == 0)
        {
            _activator.gameObject.SetActive(false);
        }
    }

    private IEnumerator MakeReagents()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(_makingReagentTime);

        yield return waitingTime;

        Reagent reagent;
        KeeperPlace[] emptyKeepers = _keepers.Where(keeper => keeper.IsEmpty).ToArray();

        foreach (KeeperPlace keeper in emptyKeepers)
        {
            reagent = _pool.TryGetDisabledReagent();
            reagent.transform.position = keeper.transform.position;
            keeper.Take(reagent.transform);
            reagent.gameObject.SetActive(true);
        }

        if (_activator.gameObject.activeSelf == false)
        {
            _activator.gameObject.SetActive(true);
        }

        _reagentMaker = null;
    }
}