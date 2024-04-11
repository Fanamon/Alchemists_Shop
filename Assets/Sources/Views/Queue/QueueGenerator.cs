using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QueueGenerator : MonoBehaviour
{
    [SerializeField] private float _generatingDelaySeconds;

    [SerializeField] private Transform _placeForGeneration;
    [SerializeField] private VisitorsPool _visitorsPool;

    [SerializeField] private List<AlchemyTablePurchaser> _alchemyTablePurchasers;

    private Coroutine _generator;
    private DiseaseRandomizer _diseaseRandomizer = new DiseaseRandomizer();

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

    public void AddNewDiseaseType(DiseaseType type)
    {
        _diseaseRandomizer.AddNewType(type);
    }

    public void ChangeVisitorsGenerationDelayInPercentage(float delayPercentage)
    {
        _generatingDelaySeconds *= delayPercentage;
    }

    private IEnumerator Generate()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(_generatingDelaySeconds);

        while (isActiveAndEnabled)
        {
            yield return waitingTime;

            VisitorGenerated(GetCreatedVisitor());
        }
    }

    private Visitor GetCreatedVisitor()
    {
        Visitor generatedVisitor = _visitorsPool.TryGetDisabledVisitor(_diseaseRandomizer.GetRandomDiseaseType());

        generatedVisitor.Reset(_placeForGeneration);

        return generatedVisitor;
    }
}
