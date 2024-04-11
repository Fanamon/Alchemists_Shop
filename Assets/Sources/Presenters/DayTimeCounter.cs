using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DayTimeCounter : MonoBehaviour
{
    [SerializeField] private QueueGenerator _queueGenerator;
    [SerializeField] private VisitorQueue _visitorQueue;
    [SerializeField] private DayTimeEffectsChanger _dayTimeEffectsChanger;

    private float _counter = 0;

    private DailyQueueLogic _dailyQueueLogic = new DailyQueueLogic();
    private WaitForSeconds _waitForSeconds;

    private Coroutine _dayTimeCounter = null;

    public event UnityAction<int> DayPassed;
    public event UnityAction<DiseaseType> DiseaseAdded;

    private void OnEnable()
    {
        _dailyQueueLogic.DayPassed += OnDayPassed;

        _dailyQueueLogic.ScabiesAdded += OnDiseaseAdded;
        _dailyQueueLogic.TyphusAdded += OnDiseaseAdded;
        _dailyQueueLogic.PlagueAdded += OnDiseaseAdded;
        _dailyQueueLogic.DepressionAdded += OnDiseaseAdded;
    }

    private void OnDisable()
    {
        _dailyQueueLogic.DayPassed -= OnDayPassed;

        _dailyQueueLogic.ScabiesAdded -= OnDiseaseAdded;
        _dailyQueueLogic.TyphusAdded -= OnDiseaseAdded;
        _dailyQueueLogic.PlagueAdded -= OnDiseaseAdded;
        _dailyQueueLogic.DepressionAdded -= OnDiseaseAdded;
    }

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(DayTimeConstants.OneSecond);

        _dailyQueueLogic.StartDayCounting();
        StartDayTime();
    }

    private void OnDiseaseAdded(DiseaseType type)
    {
        _queueGenerator.AddNewDiseaseType(type);
        DiseaseAdded?.Invoke(type);
    }

    private void OnDayPassed(int dayCount)
    {
        DayPassed?.Invoke(dayCount);
    }

    private void StartDayTime()
    {
        if (_dayTimeCounter != null)
        {
            StopCoroutine(_dayTimeCounter);
        }

        if (_dailyQueueLogic.DayTime == DayTime.Day)
        {
            _dayTimeCounter = StartCoroutine(StartDay());
        }

        if (_dailyQueueLogic.DayTime == DayTime.Night)
        {
            _dayTimeCounter = StartCoroutine(StartNight());
        }
    }

    private IEnumerator StartDay(float dayDurationInSeconds = DayTimeConstants.DayDurationInSeconds)
    {
        float dayTimePercentage;

        bool isFirstPercentagePassed = false;
        bool isSecondPercentagePassed = false;
        bool isThirdPercentagePassed = false;

        _queueGenerator.StartGenerating();
        _visitorQueue.StartRotateQueue();

        while (_counter < dayDurationInSeconds)
        {
            yield return _waitForSeconds;

            _counter++;
            dayTimePercentage = _counter / dayDurationInSeconds;
            _dayTimeEffectsChanger.MakeNightEffects(dayTimePercentage);
            TryChangeDayGeneratingDelayByDayTimeCounterPercentage(dayTimePercentage, ref isFirstPercentagePassed, 
                ref isSecondPercentagePassed, ref isThirdPercentagePassed);
        }

        _queueGenerator.StopGenerating();
        _visitorQueue.StopRotateQueue();

        _counter = 0;
        _dayTimeCounter = null;
        _dailyQueueLogic.PassDay();
        StartDayTime();
    }

    private void TryChangeDayGeneratingDelayByDayTimeCounterPercentage(float dayTimePercentage, ref bool isFirstPercentagePassed,
        ref bool isSecondPercentagePassed, ref bool isThirdPercentagePassed)
    {
        if (dayTimePercentage > DayTimeConstants.DayThirdPercentageToChangeVisitorsGenerating && isThirdPercentagePassed == false)
        {
            _queueGenerator.ChangeVisitorsGenerationDelayInPercentage(DayTimeConstants.DayThirdPercentageGeneratingDelay);
            isThirdPercentagePassed = true;
        }
        else if (dayTimePercentage > DayTimeConstants.DaySecondPercentageToChangeVisitosGenerating && isSecondPercentagePassed == false)
        {
            _queueGenerator.ChangeVisitorsGenerationDelayInPercentage(DayTimeConstants.DaySecondPercentageGeneratingDelay);
            isSecondPercentagePassed = true;
        }
        else if (dayTimePercentage > DayTimeConstants.DayFirstPercentageToChangeVisitorsGenerating && isFirstPercentagePassed == false)
        {
            _queueGenerator.ChangeVisitorsGenerationDelayInPercentage(DayTimeConstants.DayFistPercentageGeneratingDelay);
            isFirstPercentagePassed = true;
        }
    }

    private IEnumerator StartNight(float nightDurationInSeconds = DayTimeConstants.NightDurationInSeconds)
    {
        float dayTimePercentage;

        bool isFirstPercentagePassed = false;
        bool isSecondPercentagePassed = false;
        bool isThirdPercentagePassed = false;

        _queueGenerator.StartGenerating();
        _visitorQueue.StartRotateQueue();

        while (_counter < nightDurationInSeconds)
        {
            yield return _waitForSeconds;

            _counter++;
            dayTimePercentage = _counter / nightDurationInSeconds;
            _dayTimeEffectsChanger.MakeDayEffects(dayTimePercentage);
            TryChangeNightGeneratingDelayByDayTimeCounterPercentage(dayTimePercentage, ref isFirstPercentagePassed,
                ref isSecondPercentagePassed, ref isThirdPercentagePassed);
        }

        _queueGenerator.StopGenerating();
        _visitorQueue.StopRotateQueue();

        _counter = 0;
        _dayTimeCounter = null;
        _dailyQueueLogic.PassNight();
        StartDayTime();
    }

    private void TryChangeNightGeneratingDelayByDayTimeCounterPercentage(float dayTimePercentage, ref bool isFirstPercentagePassed,
        ref bool isSecondPercentagePassed, ref bool isThirdPercentagePassed)
    {
        if (dayTimePercentage > DayTimeConstants.NightThirdPercentageToChangeVisitorsGenerating && isThirdPercentagePassed == false)
        {
            _queueGenerator.ChangeVisitorsGenerationDelayInPercentage(DayTimeConstants.NightThirdPercentageGeneratingDelay);
            isThirdPercentagePassed = true;
        }
        else if (dayTimePercentage > DayTimeConstants.NightSecondPercentageToChangeVisitosGenerating && isSecondPercentagePassed == false)
        {
            _queueGenerator.ChangeVisitorsGenerationDelayInPercentage(DayTimeConstants.NightSecondPercentageGeneratingDelay);
            isSecondPercentagePassed = true;
        }
        else if (dayTimePercentage > DayTimeConstants.NightFirstPercentageToChangeVisitorsGenerating && isFirstPercentagePassed == false)
        {
            _queueGenerator.ChangeVisitorsGenerationDelayInPercentage(DayTimeConstants.NightFistPercentageGeneratingDelay);
            isFirstPercentagePassed = true;
        }
    }
}