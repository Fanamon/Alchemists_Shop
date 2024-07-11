using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PotionDrinker))]
public class VisitorModelsAnimationsView : MonoBehaviour
{
    [SerializeField] private GameObject _human;
    [SerializeField] private GameObject _pig;
    [SerializeField] private GameObject _sheep;
    [SerializeField] private GameObject _chiken;

    [SerializeField] private Renderer _humanRenderer;
    [SerializeField] private VisitorDiseaseView _visitorDiseaseView;
    [SerializeField] private Material _healthHumanMaterial;
    [SerializeField] private float _changeColorTime = 2f;

    [SerializeField] private ParticleSystem _dustParticleSystem;

    private AnimalRandomizer _randomizer;

    private Animator _humanAnimator;
    private Animator _pigAnimator;
    private Animator _sheepAnimator;
    private Animator _chikenAnimator;

    private PotionDrinker _potionDrinker;
    private Material _humanSkinMaterial;

    private Color _healthHumanColor;
    private Color _defaultVisitorColor;

    public event UnityAction DrinkingFinished;

    public float DrinkingTime { get; private set; }

    private void Awake()
    {
        _randomizer = new AnimalRandomizer();

        _humanAnimator = _human.GetComponent<Animator>();
        _pigAnimator = _pig.GetComponent<Animator>();
        _sheepAnimator = _sheep.GetComponent<Animator>();
        _chikenAnimator = _chiken.GetComponent<Animator>();

        _potionDrinker = GetComponent<PotionDrinker>();
        //_humanSkinMaterial = _humanRenderer.material;

        _healthHumanColor = _healthHumanMaterial.color;
        //_defaultVisitorColor = _humanSkinMaterial.color;
    }

    private void OnEnable()
    {
        _potionDrinker.Cured += OnCured;
        _potionDrinker.Failed += OnFailed;

        _visitorDiseaseView.enabled = true;
    }

    private void OnDisable()
    {
        ResetVisitorView();

        _potionDrinker.Cured -= OnCured;
        _potionDrinker.Failed -= OnFailed;
    }

    private void Start()
    {
        DrinkingTime = TryGetAnimation(_humanAnimator, AnimatorParameters.Drink).length;
    }

    public void Drink()
    {
        _humanAnimator.SetTrigger(AnimatorParameters.Drink);
    }

    public void Walk()
    {
        Animator walkAnimator = GetEnabledModelAnimator();

        walkAnimator.SetFloat(AnimatorParameters.Speed, AnimatorParameters.MoveSpeed);
    }

    public void Idle()
    {
        Animator walkAnimator = GetEnabledModelAnimator();

        walkAnimator.SetFloat(AnimatorParameters.Speed, AnimatorParameters.IdleSpeed);
    }

    private Animator GetEnabledModelAnimator()
    {
        Animator walkAnimator;

        if (_human.activeSelf)
        {
            walkAnimator = _humanAnimator;
        }
        else if (_pig.activeSelf)
        {
            walkAnimator = _pigAnimator;
        }
        else if (_sheep.activeSelf)
        {
            walkAnimator = _sheepAnimator;
        }
        else
        {
            walkAnimator = _chikenAnimator;
        }

        return walkAnimator;
    }

    private void OnCured(PotionDrinker potionDrinker)
    {
        _visitorDiseaseView.enabled = false;

        StartCoroutine(ChangeMaterialColor(_healthHumanColor));
    }

    private void OnFailed()
    {
        _visitorDiseaseView.OnFailed();

        StartCoroutine(TurnIntoAnimal());
    }

    private IEnumerator ChangeMaterialColor(Color targetColor)
    {
        WaitForFixedUpdate waitingTime = new WaitForFixedUpdate();

        for (float i = 0.01f; i < _changeColorTime; i += 0.1f)
        {
            //_humanSkinMaterial.color = Color.Lerp(_humanSkinMaterial.color, _healthHumanColor, i / _changeColorTime);

            yield return waitingTime;
        }

        DrinkingFinished?.Invoke();
    }

    private IEnumerator TurnIntoAnimal()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(_dustParticleSystem.main.duration);

        _dustParticleSystem.Play();
        _human.SetActive(false);

        Animal randomAnimal = _randomizer.GetRandomAnimal();

        switch (randomAnimal)
        {
            case Animal.Pig:
                _pig.SetActive(true);
                break;

            case Animal.Sheep:
                _sheep.SetActive(true);
                break;

            case Animal.Chicken:
                _chiken.SetActive(true);
                break;
        }

        yield return waitingTime;

        DrinkingFinished?.Invoke();
    }

    private void ResetVisitorView()
    {
        _human.SetActive(true);
        _pig.SetActive(false);
        _sheep.SetActive(false);
        _chiken.SetActive(false);

        //_humanSkinMaterial.color = _defaultVisitorColor;
    }

    private AnimationClip TryGetAnimation(Animator animator, string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }
}

public enum Animal
{
    Pig = 0,
    Sheep = 1,
    Chicken = 2
}