using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTurnTimerUI : MonoBehaviour
{
    [SerializeField] private PlayerTurnTimer _timer;

    [SerializeField] private TextMeshProUGUI _timeText;

    private void Start()
    {
        if (!_timer.IsOwner)
        {
            _timeText.enabled = false;
            return;
        }

        _timer.TimerStarted += OnTimerStarted;
        _timer.TimerStopped += OnTimerStopped;
        _timer.TimerValueChanged += OnTimerValueChanged;
    }

    private void OnTimerValueChanged(int seconds)
    {
        _timeText.enabled = true;

        _timeText.text = seconds.ToString("00:00");
    }

    private void OnTimerStopped()
    {
        _timeText.enabled = true;
    }

    private void OnTimerStarted()
    {
        _timeText.enabled = false;
    }

    private void OnDisable()
    {
        _timer.TimerStarted -= OnTimerStarted;
        _timer.TimerStopped -= OnTimerStopped;
        _timer.TimerValueChanged -= OnTimerValueChanged;
    }
}