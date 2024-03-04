using System;
using System.Collections.Generic;

public class DiseaseRandomizer
{
    private Random _random = new Random();

    private List<DiseaseType> _enabledDiseaseTipes = new List<DiseaseType>();

    public DiseaseRandomizer() { }

    public void AddNewType(DiseaseType type)
    {
        _enabledDiseaseTipes.Add(type);
    }

    public DiseaseType GetRandomDiseaseType()
    {
        int randomIndex = _random.Next(_enabledDiseaseTipes.Count);

        return _enabledDiseaseTipes[randomIndex];
    }
}