using TMPro;
using UnityEngine;

public class DayCountView : MonoBehaviour
{
    private const string DayCountPreText = "Day: ";

    [SerializeField] private TMP_Text _dayCountText;
    [SerializeField] private DayTimeCounter _dayTimeCounter;

    private void OnEnable()
    {
        _dayTimeCounter.DayPassed += OnDayPassed;
    }

    private void OnDisable()
    {
        _dayTimeCounter.DayPassed -= OnDayPassed;
    }

    private void OnDayPassed(int dayCount)
    {
        _dayCountText.text = DayCountPreText + dayCount;
    }
}
