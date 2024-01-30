using UnityEngine;
using UnityEngine.Events;

public class VisitorActivator : MonoBehaviour
{
    public event UnityAction<Visitor> VisitorEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Visitor visitor))
        {
            VisitorEntered?.Invoke(visitor);
        }
    }
}
