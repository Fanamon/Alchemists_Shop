using UnityEngine;

public class VisitorDiseaseView : MonoBehaviour
{
    [SerializeField] private float _minCoughDelay = 15f;
    [SerializeField] private float _maxCoughDelay = 20f;

    [SerializeField] private Animator _humanAnimator;
    [SerializeField] private ParticleSystem _particleSystem;

    private float _currentCoughCounter;
    private float _randomCoughDelay;

    private void OnEnable()
    {
        ResetCounter();

        _particleSystem.Play();
    }

    private void OnDisable()
    {
        _particleSystem.Stop();
    }

    private void Update()
    {
        if (_currentCoughCounter >= _randomCoughDelay)
        {
            _humanAnimator.SetTrigger(AnimatorParameters.Cough);

            ResetCounter();
        }
        else
        {
            _currentCoughCounter += Time.deltaTime;
        }
    }

    public void OnFailed()
    {
        _particleSystem.Pause();
        _particleSystem.Clear();
    }

    private void ResetCounter()
    {
        _currentCoughCounter = 0;
        _randomCoughDelay = Random.Range(_minCoughDelay, _maxCoughDelay);
    }
}
