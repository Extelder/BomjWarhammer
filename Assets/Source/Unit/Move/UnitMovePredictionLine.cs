using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitMove), typeof(Unit))]
public class UnitMovePredictionLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;

    private UnitMove _move;
    private Unit _unit;

    private UnitMovePrediction _movePrediction;

    private void Awake()
    {
        _move = GetComponent<UnitMove>();
        _unit = GetComponent<Unit>();
        _move.Bootstrapped += OnMoveBootstrapped;
    }

    private void OnMoveBootstrapped()
    {
        _move = GetComponent<UnitMove>();
        _movePrediction = _move.MovePrediction;

        _movePrediction.PredictionCompleated += OnPredictionCompleated;
        _movePrediction.PredictionCleared += OnPredictionCleared;

        _unit.UnitDeSelected += OnUnitDeSelected;
    }

    private void OnUnitDeSelected()
    {
        ClearPath();
    }

    private void OnPredictionCleared()
    {
        ClearPath();
    }

    private void ClearPath()
    {
        _line.positionCount = 1;
        _line.SetPosition(0, Vector3.zero);
    }

    private void OnPredictionCompleated(bool pathValid)
    {
        if (!pathValid)
            return;
        DrawPath(_movePrediction.CurrentPath);
    }

    private void DrawPath(NavMeshPath path)
    {
        _line.SetPosition(0, transform.position);

        if (path.corners.Length < 2)
            return;

        _line.SetVertexCount(path.corners.Length);

        for (var i = 1; i < path.corners.Length; i++)
        {
            _line.SetPosition(i, path.corners[i]);
        }
    }

    private void OnDisable()
    {
        _move.Bootstrapped -= OnMoveBootstrapped;
        _movePrediction.PredictionCompleated -= OnPredictionCompleated;
        _movePrediction.PredictionCleared -= OnPredictionCleared;


        _unit.UnitDeSelected -= OnUnitDeSelected;
    }
}