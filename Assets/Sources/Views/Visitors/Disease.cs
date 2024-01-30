using UnityEngine;

public class Disease : MonoBehaviour
{
    [SerializeField] private DiseaseType _type;
    [SerializeField] private Potion _potionToCure;

    public DiseaseType Type => _type;
}

public enum DiseaseType
{
    Influenza = 0,
    Scabies = 1,
    Typhus = 2,
    Plague = 3,
    Depression = 4,
}