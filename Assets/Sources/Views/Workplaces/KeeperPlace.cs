using System.Collections;
using UnityEngine;

public class KeeperPlace : MonoBehaviour, IKeepable
{
    [SerializeField] private float _pickingUpSpeed = 3f;

    [SerializeField] private Transform _startingObjectToTake = null;

    private Transform _transform;
    private Transform _objectToKeep;

    private bool _isEmpty;

    public bool IsEmpty => _isEmpty;

    private void Awake()
    {
        _transform = transform;
        
        if (_startingObjectToTake != null)
        {
            Take(_startingObjectToTake);
        }
        else
        {
            _isEmpty = true;
        }
    }

    public void Take(Transform objectToKeep)
    {
        _isEmpty = false;
        objectToKeep.SetParent(_transform);

        StartCoroutine(PickUp(objectToKeep));
    }

    public Transform GiveAway()
    {
        ValidateObjectToKeep();
        Transform objectToGiveAway = _objectToKeep;

        _objectToKeep = null;
        _isEmpty = true;

        return objectToGiveAway;
    }

    public GameObject ShowObjectInKeep()
    {
        ValidateObjectToKeep();

        return _objectToKeep.gameObject;
    }

    public void DropObject()
    {
        if (_objectToKeep != null)
        {
            Destroy(_objectToKeep.gameObject);
            _objectToKeep = null;
            _isEmpty = true;
        }
    }

    private void ValidateObjectToKeep()
    {
        if (_objectToKeep == null)
        {
            throw new System.ArgumentNullException("Place doesn't have object to keep.");
        }
    }

    private IEnumerator PickUp(Transform objectToTake)
    {
        while (objectToTake.position != _transform.position)
        {
            objectToTake.position = Vector3.MoveTowards(objectToTake.position, _transform.position, 
                _pickingUpSpeed * Time.deltaTime);

            yield return null;
        }

        _objectToKeep = objectToTake;
    }
}

public interface IKeepable
{
    bool IsEmpty { get; }

    void Take(Transform objectToKeep);
    Transform GiveAway();
    GameObject ShowObjectInKeep();
}