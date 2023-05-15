using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestTimer : Timer
{

    private float _restTime = 0;

    [SerializeField] private Image _restTimerImage;
    [SerializeField] private TextMeshProUGUI _restTimerText;
    [SerializeField] private AudioClip _endOfSetSound;
    public static Action OnRestStart;
    public static Action _hasEnded;
    private bool _timerHasEnded = true;
    private bool _startedFirstRest = false;
    private SetTimer _setTimer;
    private void Start()
    {
        _timer = 0;
        UpdateTimerText("0");
        _setTimer = FindObjectOfType<SetTimer>();
    }
    public override void Countdown()
    {
        if (_timer > 0)
        {
            _timer -= 1f * Time.deltaTime;
            _restTimerImage.fillAmount = _timer / (_restTime * 1f);
            UpdateTimerText(_timer.ToString("0"));
        }
        else if (_timer <= 0 && !_timerHasEnded  )
        {
            _restTimerImage.fillAmount = _timer / (_restTime * 1f);
            if (!_timerHasEnded) { _hasEnded?.Invoke(); _restTimerImage.fillAmount = 1; }
            
            if (Settings._startSetTimerAutomatically && _startedFirstRest)
            {
                _setTimer.StartSetTimer();
                PlayStartSetSound();
            }
            UpdateTimerText(_restTime.ToString()) ;
            _timerHasEnded = true;
        }


    }

    public void StartRest()
    {
        if (ExerciseManager.HasEnded && !ExerciseManager.Instance.IsTimerOnlyMode) { GameObject.FindObjectOfType<ExerciseManager>().EndWorkout(); return; }
        _startedFirstRest = true;

        if (_restTime > 0 && _timerHasEnded) _timer = _restTime;
        else { return; }
        _timerHasEnded = false;
        
        _setTimer.EndSetTimer();
        OnRestStart?.Invoke();
       

    }
    private void PlayStartSetSound()
    {
        AudioManager.Instance.PlaySoundFX(_endOfSetSound);
    }
    public void IncrementRestTime(float amount)
    {
        if (!_timerHasEnded) return; 

        _restTime += amount;

        if (_restTime < 0) _restTime = 0;
        UpdateTimerText(_restTime.ToString());
    }

    private void UpdateTimerText(string valueToChange)
    {
        if (ExerciseManager.HasEnded && !ExerciseManager.Instance.IsTimerOnlyMode) _restTimerText.SetText("End");
        else _restTimerText.SetText(valueToChange);
    }
}
