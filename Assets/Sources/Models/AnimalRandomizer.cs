using System;

public class AnimalRandomizer
{
    private Random _random;

    public AnimalRandomizer()
    {
        _random = new Random();
    }

    public Animal GetRandomAnimal()
    {
        int randomNumber = _random.Next(Enum.GetValues(typeof(Animal)).Length);

        return (Animal)randomNumber;
    }
}