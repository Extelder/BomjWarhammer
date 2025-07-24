using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStepsLeftUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _attackLeftText;
    [SerializeField] private TextMeshProUGUI _moveStepsLeftText;

    [SerializeField] private PlayerTurnChanger _turnChanger;

    private void OnEnable()
    {
        _turnChanger.AttackStepsValueChanged += OnAttackStepsValueChanged;
        _turnChanger.MoveStepsValueChanged += OnMoveStepsValueChanged;
    }

    private void OnMoveStepsValueChanged(int value)
    {
        OnStepsValueChanged(value, _moveStepsLeftText);
    }

    private void OnAttackStepsValueChanged(int value)
    {
        OnStepsValueChanged(value, _attackLeftText);
    }

    private void Start()
    {
        if (!_turnChanger.IsOwner)
        {
            _attackLeftText.enabled = false;
            _moveStepsLeftText.enabled = false;
        }
    }


    private void OnStepsValueChanged(int value, TextMeshProUGUI text)
    {
        text.text = (value).ToString();
    }

    private void OnDisable()
    {
        _turnChanger.AttackStepsValueChanged -= OnAttackStepsValueChanged;
        _turnChanger.MoveStepsValueChanged -= OnMoveStepsValueChanged;
    }
}