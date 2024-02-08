using System;

public class DiseaseRandomizer
{
    private Random _random = new Random();

    public DiseaseRandomizer() { }

    public DiseaseType GetRandomDiseaseType()
    {
        return (DiseaseType)_random.Next((int)DiseaseType.Scabies + 1);
    }
}