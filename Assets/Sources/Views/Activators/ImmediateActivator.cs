using UnityEngine;
using UnityEngine.Events;

public class ImmediateActivator : MonoBehaviour
{
    public event UnityAction<Player> PlayerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            PlayerEntered?.Invoke(player);
        }
    }
}
