using UnityEngine;

public class ExitPlaceActivator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Visitor visitor))
        {
            visitor.gameObject.SetActive(false);
        }
    }
}
