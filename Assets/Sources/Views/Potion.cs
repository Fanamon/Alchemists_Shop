using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private DiseaseType _diseaseType;

    public DiseaseType DiseaseType => _diseaseType;
}
