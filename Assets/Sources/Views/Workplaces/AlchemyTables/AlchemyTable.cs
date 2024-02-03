using System.Collections;
using System.Linq;
using UnityEngine;

public class AlchemyTable : MonoBehaviour
{
    [SerializeField] private float _makingPotionTime;

    [SerializeField] private DiseaseType _diseaseType;

    [SerializeField] private ImmediateActivator _activator;
    [SerializeField] private PotionPool _pool;
    [SerializeField] private KeeperPlace[] _keepers;

    private Coroutine _potionMaker = null;

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
            _potionMaker = StartCoroutine(MakePotions());
        }
        else if (_keepers.Where(keeper => keeper.IsEmpty == false).Count() == 0)
        {
            _activator.gameObject.SetActive(false);
        }
    }

    private void OnPlayerEntered(Player player)
    {
        player.TryTake(_keepers.Where(keeper => keeper.IsEmpty == false).ToArray());

        if (_potionMaker == null)
        {
            _potionMaker = StartCoroutine(MakePotions());
        }

        if (_keepers.Where(keeper => keeper.IsEmpty == false).Count() == 0)
        {
            _activator.gameObject.SetActive(false);
        }
    }
    
    private IEnumerator MakePotions()
    {
        WaitForSecondsRealtime waitingTime = new WaitForSecondsRealtime(_makingPotionTime);

        yield return waitingTime;

        Potion potion;
        KeeperPlace[] emptyKeepers = _keepers.Where(keeper => keeper.IsEmpty).ToArray();
        
        foreach (KeeperPlace keeper in emptyKeepers)
        {
            potion = _pool.TryGetDisabledPotion(_diseaseType);
            potion.transform.position = keeper.transform.position;
            keeper.Take(potion.transform);
            potion.gameObject.SetActive(true);
        }

        if (_activator.gameObject.activeSelf == false)
        {
            _activator.gameObject.SetActive(true);
        }

        _potionMaker = null;
    }
}