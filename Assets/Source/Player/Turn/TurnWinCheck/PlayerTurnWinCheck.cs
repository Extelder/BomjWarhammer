using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnWinCheck : MonoBehaviour
{
    [SerializeField] private UnitTeam _blueTeam;
    [SerializeField] private UnitTeam _redTeam;

    [SerializeField] private int _turnToCheck;

    [SerializeField] private UnitTurn _turn;

    private List<Unit> _redUnits = new List<Unit>();
    private List<Unit> _blueUnits = new List<Unit>();

    public event Action<UnitTeam> TeamWin;

    private void OnEnable()
    {
        _turn.TurnChanged += OnTurnChanged;
    }

    private void OnTurnChanged()
    {
        if (_turn.CurrentTurnIteration >= _turnToCheck)
        {
            Unit[] units = FindObjectsByType<Unit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            _redUnits.Clear();
            _blueUnits.Clear();

            for (int i = 0; i < units.Length; i++)
            {
                if (units[i] == null)
                    continue;
                if (units[i].Team == _blueTeam)
                {
                    _blueUnits.Add(units[i]);
                    continue;
                }

                _redUnits.Add(units[i]);
            }

            if (_redUnits.Count > _blueUnits.Count)
            {
                TeamWin?.Invoke(_redTeam);
                Debug.Log("Red wins");
            }

            if (_blueUnits.Count > _redUnits.Count)
            {
                TeamWin?.Invoke(_blueTeam);
                Debug.Log("Blue wins");
            }
        }
    }

    private void OnDisable()
    {
        _turn.TurnChanged -= OnTurnChanged;
    }
}