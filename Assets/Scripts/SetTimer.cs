using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SetTimer : Timer
{
    [SerializeField] private TextMeshProUGUI _setTimerText;
        
    private bool _startSetTimer = false;
    [SerializeField] private TextMeshProUGUI _lastSetTimeText;
    private int minutes = 0;

    public static Action OnTimerStart;
    public static Action OnTimerEnd;
    public override void Countdown()
    {
        if (_startSetTimer)
        {
            _timer += 1f * Time.deltaTime;
            if (_timer >= 60)
            {
                minutes++;
                _timer = 0;
            }
        }
        else if (_timer != 0)
        {
            _lastSetTimeText.SetText($" {minutes}:{_timer.ToString("0.0")} seconds");
            minutes = 0;
            _timer = 0;
            
        }
        

        _setTimerText.SetText($"{minutes}:{_timer.ToString("0.0")} seconds");
    }

    public void SetButton() {
        if (_startSetTimer) EndSetTimer();
        else StartSetTimer();
    
    }

    public void StartSetTimer() { _startSetTimer = true; OnTimerStart?.Invoke(); }

    public void EndSetTimer() { if (_startSetTimer) {OnTimerEnd?.Invoke();  }  _startSetTimer = false; }

   
}
