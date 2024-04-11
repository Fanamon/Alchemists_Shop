using UnityEngine;

public class DayTimeEffectsChanger : MonoBehaviour
{
    [SerializeField] private Light _sunlight;

    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;

    public void MakeDayEffects(float currentTimePercentage)
    {
        _sunlight.color = Color.Lerp(_nightColor, _dayColor, currentTimePercentage);
    }

    public void MakeNightEffects(float currentTimePercentage)
    {
        _sunlight.color = Color.Lerp(_dayColor, _nightColor, currentTimePercentage);
    }
}
