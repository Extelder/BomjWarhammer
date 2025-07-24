using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMove), typeof(UnitAttack), typeof(UnitHitBox))]
[RequireComponent(typeof(UnitHealth))]
public class Unit : MonoBehaviour
{
    [field: SerializeField] public UnitTeam Team { get; private set; }

    private UnitHealth _health;
    private UnitHitBox _hitBox;

    private UnitMove _move;
    private UnitMovePrediction _movePrediction;
    private UnitAttack _attack;

    public event Action UnitSelected;
    public event Action UnitDeSelected;

    public bool Selected { get; private set; }

    private void Awake()
    {
        _move = GetComponent<UnitMove>();
        _attack = GetComponent<UnitAttack>();
        _health = GetComponent<UnitHealth>();
        _hitBox = GetComponent<UnitHitBox>();
    }

    private void OnEnable()
    {
        _move.Bootstrapped += OnMoveBootstrapped;
        _hitBox.Hitted += _health.TakeDamage;
    }

    private void OnMoveBootstrapped()
    {
        _movePrediction = _move.MovePrediction;
    }

    public void Select()
    {
        Selected = true;
        UnitSelected?.Invoke();
    }

    public void DeSelect()
    {
        Selected = false;
        UnitDeSelected?.Invoke();
    }


    public void VizualizeMovePath(Vector3 point)
    {
        _movePrediction.Predict(point);
    }

    public void TryMove()
    {
        _move.TryMove();
    }

    public void TryAttack(UnitHitBox unitToAttack)
    {
        _attack.TryAttack(unitToAttack);
    }

    private void OnDisable()
    {
        _move.Bootstrapped -= OnMoveBootstrapped;
        _hitBox.Hitted -= _health.TakeDamage;
    }
}