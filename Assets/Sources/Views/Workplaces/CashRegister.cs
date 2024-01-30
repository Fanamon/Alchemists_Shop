using System.Linq;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    private const int OneHand = 1;

    [SerializeField] private ActivatorWithDelay _activator;
    [SerializeField] private VisitorActivator _visitorActivator;

    private Visitor _visitor;

    private void Awake()
    {
        _activator.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _activator.Activated += OnActivated;
        _visitorActivator.VisitorEntered += OnVisitorEntered;
    }

    private void OnDisable()
    {
        _activator.Activated -= OnActivated;
        _visitorActivator.VisitorEntered -= OnVisitorEntered;
    }

    private void OnActivated(Player player)
    {
        KeeperPlace[] handsWithPotions = player.TryGetKeepersWithObjects().
            Where(hand => hand.ShowObjectInKeep().TryGetComponent(out Potion potion)).ToArray();
        KeeperPlace handWithRequiredPotion = handsWithPotions.FirstOrDefault(hand =>
            hand.ShowObjectInKeep().GetComponent<Potion>().DiseaseType == _visitor.GetComponent<Disease>().Type);

        if (handsWithPotions.Length == 0)
        {
            return;
        }

        if (handsWithPotions.Length > OneHand && handWithRequiredPotion != null)
        {
            _visitor.TakePotion(handWithRequiredPotion.GiveAway());
        }
        else
        {
            _visitor.TakePotion(handsWithPotions.First().GiveAway());
        }

        _activator.gameObject.SetActive(false);
    }

    private void OnVisitorEntered(Visitor visitor)
    {
        _activator.gameObject.SetActive(true);
        _visitor = visitor;
    }
}
