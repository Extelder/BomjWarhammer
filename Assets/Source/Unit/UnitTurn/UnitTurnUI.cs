using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitTurnUI : MonoBehaviour
{
    [SerializeField] private PlayerTurnChanger _turnChanger;

    [SerializeField] private UnitTurn _turn;

    [SerializeField] private TextMeshProUGUI _turnCountText;
    [SerializeField] private TextMeshProUGUI _currentTurnText;

    private void OnEnable()
    {
        _turnChanger.TurnChangedToCurrent += OnTurnChangedToCurrent;
        _turnChanger.TurnChangedToAnother += OnTurnChangedToAnother;
    }

    private void OnTurnChangedToCurrent()
    {
        Debug.Log("Current");
        _currentTurnText.enabled = true;
    }

    private void OnTurnChangedToAnother()
    {
        Debug.Log("Another");
        _currentTurnText.enabled = false;
    }

    private void Start()
    {
        if (!_turn.IsOwner)
        {
            _turnCountText.enabled = false;
            Destroy(this);
            return;
        }

        _turn.TurnChanged += OnTurnChanged;
    }

    private void OnTurnChanged()
    {
        _turnCountText.text = _turn.CurrentTurnIteration.ToString();
    }

    private void OnDisable()
    {
        _turn.TurnChanged -= OnTurnChanged;
        _turnChanger.TurnChangedToCurrent -= OnTurnChangedToCurrent;
        _turnChanger.TurnChangedToAnother -= OnTurnChangedToAnother;
    }
}