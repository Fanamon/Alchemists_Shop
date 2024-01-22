using UnityEngine;

public abstract class Activator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Activate(player);
        }
    }

    protected abstract void Activate(Player player);
}
