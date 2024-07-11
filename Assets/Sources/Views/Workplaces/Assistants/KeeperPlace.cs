using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KeeperPlace : MonoBehaviour, IKeepable
{
    [SerializeField] private float _pickingUpSpeed = 3f;

    [SerializeField] private Transform _startingObjectToTake = null;

    private Transform _transform;
    protected Transform ObjectToKeep;

    public event UnityAction Took;
    public event UnityAction GaveAway;

    public bool IsEmpty { get; protected set; }

    private void Awake()
    {
        _transform = transform;
        
        if (_startingObjectToTake != null)
        {
            Take(_startingObjectToTake);
        }
        else
        {
            IsEmpty = true;
        }
    }

    public void Take(Transform objectToKeep)
    {
        IsEmpty = false;
        Took?.Invoke();
        objectToKeep.SetParent(_transform);

        StartCoroutine(PickUp(objectToKeep));
    }

    public Transform GiveAway()
    {
        ValidateObjectToKeep();
        Transform objectToGiveAway = ObjectToKeep;

        ObjectToKeep = null;
        IsEmpty = true;
        GaveAway?.Invoke();

        return objectToGiveAway;
    }

    public GameObject ShowObjectInKeep()
    {
        ValidateObjectToKeep();

        return ObjectToKeep.gameObject;
    }

    public virtual void DropObject()
    {
        if (ObjectToKeep != null)
        {
            ObjectToKeep.gameObject.SetActive(false);
            ObjectToKeep = null;
            IsEmpty = true;
        }
    }

    private void ValidateObjectToKeep()
    {
        if (ObjectToKeep == null)
        {
            throw new System.ArgumentNullException("Place doesn't have object to keep.");
        }
    }

    private IEnumerator PickUp(Transform objectToTake)
    {
        ObjectToKeep = objectToTake;

        while (objectToTake.localPosition != Vector3.zero ||
            objectToTake.localRotation != Quaternion.identity)
        {
            objectToTake.localPosition = Vector3.MoveTowards(objectToTake.localPosition, Vector3.zero, 
                _pickingUpSpeed * Time.deltaTime);
            objectToTake.localRotation = Quaternion.identity;

            yield return null;
        }
    }
}

public interface IKeepable
{
    bool IsEmpty { get; }

    void Take(Transform objectToKeep);
    Transform GiveAway();
    GameObject ShowObjectInKeep();
}