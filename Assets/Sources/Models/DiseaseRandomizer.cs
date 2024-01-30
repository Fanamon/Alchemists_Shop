using System;

public class DiseaseRandomizer
{
    private Random _random = new Random();

    public DiseaseRandomizer() { }

    public DiseaseType GetRandomDiseaseType()
    {
        return DiseaseType.Influenza;//(DiseaseType)_random.Next((int)DiseaseType.Depression + 1);
    }
}