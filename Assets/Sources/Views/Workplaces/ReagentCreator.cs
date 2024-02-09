using System.Collections;
using System.Linq;
using UnityEngine;

public class ReagentCreator : MonoBehaviour
{
    [SerializeField] private float _makingReagentTime;

    [SerializeField] private ImmediateActivator _activator;
    [SerializeField] private ReagentPool _pool;
    [SerializeField] private KeeperPlace[] _keepers;

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
        if (_keepers.Where(keeper => keeper.IsEmpty).Count() != 0)
        {
            _reagentMaker = StartCoroutine(MakeReagents());
        }
        else if (_keepers.Where(keeper => keeper.IsEmpty == false).Count() == 0)
        {
            _activator.gameObject.SetActive(false);
        }
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