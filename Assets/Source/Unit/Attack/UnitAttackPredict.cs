using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackPredict
{
    private UnitAttackSettings _attackSettings;
    private UnitMovePrediction _movePrediction;

    private UnitMove _move;

    private Unit _unit;

    public event Action<Vector3, float> AttackPredicted;
    public event Action AttackPredictedCleared;

    private List<UnitHitBox> PredictedHitBoxes = new List<UnitHitBox>();

    public UnitAttackPredict(UnitMove move, UnitAttackSettings attackSettings, Unit unit)
    {
        _move = move;
        _unit = unit;
        _movePrediction = _move.MovePrediction;
        _attackSettings = attackSettings;
        _movePrediction.PredictionCompleated += OnPredictionCompleated;

        _move.DestinationCompleated += OnDestinationCompleated;
        _move.DestinationSetted += OnDestinationSetted;

        _unit.UnitSelected += OnUnitSelected;
        _unit.UnitDeSelected += OnUnitDeSelected;

        AttackPredicted += OnAttackPredicted;
        AttackPredictedCleared += OnAttackPredictedCleared;
    }

    private void OnAttackPredicted(Vector3 point, float range)
    {
        PredictedHitBoxes.Clear();
        Collider[] others = new Collider[_attackSettings.MaxObjectsToOverlap];
        Physics.OverlapSphereNonAlloc(_unit.transform.position, _attackSettings.Range, others,
            _attackSettings.AttackLayerMask);

        foreach (var other in others)
        {
            if (other == null)
                continue;
            if (other.TryGetComponent<UnitHitBox>(out UnitHitBox hitBox))
            {
                if (hitBox.Unit.Team == _unit.Team)
                    continue;

                PredictedHitBoxes.Add(hitBox);
                hitBox.InAttackableZone();
            }
        }
    }

    private void OnAttackPredictedCleared()
    {
        if (PredictedHitBoxes.Count <= 0)
            return;
        foreach (var hitBox in PredictedHitBoxes)
        {
            hitBox.OutAttackableZone();
        }
    }

    private void OnDestinationSetted()
    {
        AttackPredictedCleared?.Invoke();
    }

    private void OnDestinationCompleated()
    {
        if (!_unit.Selected)
            return;
        AttackPredicted?.Invoke(_unit.transform.position, _attackSettings.Range * 2);
    }

    private void OnUnitDeSelected()
    {
        AttackPredictedCleared?.Invoke();
    }

    private void OnUnitSelected()
    {
        AttackPredicted?.Invoke(_unit.transform.position, _attackSettings.Range * 2);
    }

    private void OnPredictionCompleated(bool pathValid)
    {
        if (!pathValid)
            return;
        AttackPredicted?.Invoke(_movePrediction.CurrentPoint, _attackSettings.Range * 2);
    }

    ~UnitAttackPredict()
    {
        _unit.UnitSelected -= OnUnitSelected;
        _movePrediction.PredictionCompleated -= OnPredictionCompleated;
        _unit.UnitDeSelected -= OnUnitDeSelected;
        _move.DestinationCompleated -= OnDestinationCompleated;
        _move.DestinationSetted += OnDestinationSetted;
    }
}