using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class QueueGenerator : MonoBehaviour
{
    [SerializeField] private float _generatingDelaySeconds;

    [SerializeField] private Transform _placeForGeneration;
    [SerializeField] private VisitorsPool _visitorsPool;

    private Coroutine _generator;

    public event UnityAction<Visitor> VisitorGenerated;

    public bool IsGenerating { get; private set; }

    public void StartGenerating()
    {
        IsGenerating = true;
        _generator = StartCoroutine(Generate());
    }

    public void StopGenerating()
    {
        IsGenerating = false;
        StopCoroutine(_generator);
    }

    private IEnumerator Generate()
    {
        WaitForSecondsRealtime waitingTime = new WaitForSecondsRealtime(_generatingDelaySeconds);

        while (isActiveAndEnabled)
        {
            yield return waitingTime;

            VisitorGenerated(GetCreatedVisitor());
        }
    }

    private Visitor GetCreatedVisitor()
    {
        Visitor generatedVisitor = _visitorsPool.TryGetDisabledVisitor();

        generatedVisitor.transform.position = _placeForGeneration.transform.position;
        generatedVisitor.transform.rotation = _placeForGeneration.transform.rotation;
        generatedVisitor.gameObject.SetActive(true);

        return generatedVisitor;
    }
}
