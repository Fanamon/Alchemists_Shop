using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ModelRandomiser : MonoBehaviour
{
    [Header("Male settings")]
    [SerializeField] private List<GameObject> _maleBodies;
    [SerializeField] private List<GameObject> _maleHeads;

    [Header("Female settings")]
    [SerializeField] private List<GameObject> _femaleBodies;
    [SerializeField] private List<GameObject> _femaleHeads;

    private Random _random;

    private void Awake()
    {
        _random = new Random();
    }

    private void OnEnable()
    {
        ResetModels();

        RandomiseModel();
    }

    private void RandomiseModel()
    {
        int randomNumber = _random.Next(Enum.GetNames(typeof(Gender)).Length);

        switch((Gender)randomNumber)
        {
            case Gender.Male:
                RandomiseGenderModel(_maleBodies, _maleHeads);
                break;

            case Gender.Female:
                RandomiseGenderModel(_femaleBodies, _femaleHeads);
                break;
        }
    }

    private void RandomiseGenderModel(List<GameObject> bodies, List<GameObject> heads)
    {
        int randomBodyNumber = _random.Next(bodies.Count);
        int randomHeadNumber = _random.Next(heads.Count);

        bodies[randomBodyNumber].SetActive(true);
        heads[randomHeadNumber].SetActive(true);
    }

    private void ResetModels()
    {
        foreach (var body in _maleBodies)
        {
            body.SetActive(false);
        }

        foreach (var head in _maleHeads)
        {
            head.SetActive(false);
        }

        foreach (var head in _femaleHeads)
        {
            head.SetActive(false);
        }

        foreach (var body in _femaleBodies)
        {
            body.SetActive(false);
        }
    }
}

public enum Gender
{
    Male = 0,
    Female = 1
}
