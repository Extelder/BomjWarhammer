using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerTurnTimer : NetworkBehaviour
{
    [SerializeField] private PlayerTurnChanger _turnChanger;

    [SerializeField] private int _seconds = 60;

    public event Action<int> TimerValueChanged;
    public event Action TimerStarted;
    public event Action TimerStopped;

    private bool _current;

    private void Start()
    {
        if (!IsOwner)
            return;
        _turnChanger.TurnChangedToCurrent += OnTurnCurrent;
        _turnChanger.TurnChangedToAnother += OnTurnAnother;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;
        OnTurnCurrent();
    }

    private void OnTurnAnother()
    {
        _current = false;
        StopAllCoroutines();
        TimerStopped?.Invoke();
    }

    private void OnTurnCurrent()
    {
        _current = true;
        TimerStarted?.Invoke();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        for (int i = 0; i < _seconds; i++)
        {
            TimerValueChanged?.Invoke(i);
            yield return new WaitForSeconds(1);
        }

        if (_current)
        {
            _turnChanger.EndTurn();
        }

        TimerStopped?.Invoke();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _turnChanger.TurnChangedToCurrent -= OnTurnCurrent;
        _turnChanger.TurnChangedToAnother -= OnTurnAnother;
    }
}