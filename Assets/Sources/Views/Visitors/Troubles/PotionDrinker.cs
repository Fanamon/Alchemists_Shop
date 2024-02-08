using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Disease))]
public class PotionDrinker : MonoBehaviour
{
    [SerializeField] private GameObject _positiveSignal;
    [SerializeField] private GameObject _negativeSignal;

    private Disease _disease;

    public event UnityAction Cured;
    public event UnityAction Failed;

    private void Awake()
    {
        _disease = GetComponent<Disease>();
        _positiveSignal.SetActive(false);
        _negativeSignal.SetActive(false);
    }

    private void OnDisable()
    {
        _positiveSignal.SetActive(false);
        _negativeSignal.SetActive(false);
    }

    public void Drink(Potion potion)
    {
        if (potion.DiseaseType == _disease.Type)
        {
            _positiveSignal.SetActive(true);
            Cured?.Invoke();
        }
        else
        {
            _negativeSignal.SetActive(true);
            Failed?.Invoke();
        }
    }
}
