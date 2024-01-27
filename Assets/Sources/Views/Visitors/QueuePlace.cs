using UnityEngine;
using UnityEngine.Events;

public class QueuePlace : MonoBehaviour
{
    public event UnityAction<QueuePlace> Emptied;

    public bool IsEmpty { get; private set; }

    private void Awake()
    {
        IsEmpty = true;
    }

    public void Take()
    {
        IsEmpty = false;
    }

    public void Leave()
    {
        IsEmpty = true;
        Emptied?.Invoke(this);
    }
}