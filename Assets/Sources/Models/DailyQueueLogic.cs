using System;

public class DailyQueueLogic
{
    private const int DayToStartScabiesVisitors = 2;
    private const int NightToStartTyphusVisitors = 3;
    private const int DayToStartPlagueVisitors = 5;
    private const int NightToStartDepressionVisitors = 7;

    private int _dayCount = 1;
    private int _nightCount = 1;

    public event Action<int> DayPassed;

    public event Action<DiseaseType> ScabiesAdded;
    public event Action<DiseaseType> TyphusAdded;
    public event Action<DiseaseType> PlagueAdded;
    public event Action<DiseaseType> DepressionAdded;

    public DayTime DayTime { get; private set; }

    public void StartDayCounting()
    {
        DayPassed?.Invoke(_dayCount);
    }

    public void PassDay()
    {
        _dayCount++;
        DayTime = DayTime.Night;

        TryExtendNightVisitors(_nightCount);
    }

    public void PassNight()
    {
        _nightCount++;
        DayTime = DayTime.Day;

        TryExtendDayVisitors(_dayCount);
        DayPassed?.Invoke(_dayCount);
    }

    private void TryExtendDayVisitors(int dayTimeCount)
    {
        if (dayTimeCount == DayToStartScabiesVisitors)
        {
            ScabiesAdded?.Invoke(DiseaseType.Scabies);
        }

        if (dayTimeCount == DayToStartPlagueVisitors)
        {
            PlagueAdded?.Invoke(DiseaseType.Plague);
        }
    }

    private void TryExtendNightVisitors(int nightCount)
    {
        if (nightCount == NightToStartTyphusVisitors)
        {
            TyphusAdded?.Invoke(DiseaseType.Typhus);
        }

        if (nightCount == NightToStartDepressionVisitors)
        {
            DepressionAdded?.Invoke(DiseaseType.Depression);
        }
    }
}

public enum DayTime
{
    Day = 0,
    Night = 1
}