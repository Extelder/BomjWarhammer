using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerTurnChanger : NetworkBehaviour
{
    [SerializeField] private UnitSelector _selector;
    [SerializeField] private UnitAttacker _attacker;
    [SerializeField] private UnitMover _mover;

    [SerializeField] private UnitTurn _turn;

    [SerializeField] private int _maxSteps = 2;

    private int _steps;
    private int _attackSteps;
    private int _moveSteps;

    public static event Action OnStepsEnded;

    public event Action TurnChangedToCurrent;
    public event Action TurnChangedToAnother;

    public event Action<int> StepsValueChanged;
    public event Action<int> AttackStepsValueChanged;
    public event Action<int> MoveStepsValueChanged;

    private void Start()
    {
        if (!IsOwner)
            return;
        _turn.TurnChanged += OnTurnChanged;
        OnTurnChanged();
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if (_turn.CurrentStepPlayerOwnId != OwnerClientId)
            return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        _attackSteps = 0;
        _moveSteps = 0;
        _steps = 0;
        CheckStepsEnded();
    }

    private void OnTurnChanged()
    {
        if (_turn.CurrentStepPlayerOwnId == OwnerClientId)
        {
            _steps = _maxSteps;
            _moveSteps = _steps - 1;
            _attackSteps = _steps - 1;
            StepsValueChanged?.Invoke(_steps);
            AttackStepsValueChanged?.Invoke(_attackSteps);
            MoveStepsValueChanged?.Invoke(_moveSteps);
            StepsFulled();
            UnitMove.Moved += OnUnitMoved;
            UnitAttack.Attacked += OnUnitAttacked;
            TurnChangedToCurrent?.Invoke();
        }
        else
        {
            UnitMove.Moved -= OnUnitMoved;
            UnitAttack.Attacked -= OnUnitAttacked;

            _selector.enabled = false;
            _attacker.enabled = false;
            _mover.enabled = false;
            TurnChangedToAnother?.Invoke();
        }
    }

    private void OnUnitAttacked()
    {
        _attackSteps--;
        _steps--;
        StepsValueChanged?.Invoke(_steps);
        AttackStepsValueChanged?.Invoke(_attackSteps);
        CheckStepsEnded();
    }

    private bool CheckStepsEnded()
    {
        if (_attackSteps - 1 < 0)
        {
            AttackStepsValueChanged?.Invoke(0);
            _attacker.enabled = false;
        }

        if (_moveSteps - 1 < 0)
        {
            _mover.enabled = false;
            MoveStepsValueChanged?.Invoke(0);
        }

        if (_steps - 1 < 0)
        {
            _selector.enabled = false;
            OnStepsEnded?.Invoke();
            StepsValueChanged?.Invoke(0);
            return true;
        }

        return false;
    }

    private void StepsFulled()
    {
        _selector.enabled = true;
        _attacker.enabled = true;
        _mover.enabled = true;
    }

    private void OnUnitMoved()
    {
        _moveSteps--;
        _steps--;
        MoveStepsValueChanged?.Invoke(_moveSteps);
        StepsValueChanged?.Invoke(_steps);
        CheckStepsEnded();
    }

    private void OnDisable()
    {
        UnitMove.Moved -= OnUnitMoved;
        UnitAttack.Attacked -= OnUnitAttacked;
        _turn.TurnChanged -= OnTurnChanged;
    }
}